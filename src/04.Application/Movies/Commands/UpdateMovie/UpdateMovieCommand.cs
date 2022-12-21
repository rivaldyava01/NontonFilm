using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Movies.Commands.UpdateMovie;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Movies.Commands.UpdateMovie;

[Authorize(Policy = Permissions.NontonFilm_Movie_Edit)]

public class UpdateMovieCommand : UpdateMovieRequest, IRequest
{

}
public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        Include(new UpdateMovieRequestValidator());
    }
}

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand>
{
    private readonly INontonFilmDbContext _context;

    public UpdateMovieCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.MovieGenres)
            .SingleOrDefaultAsync(cancellationToken);

        if (movie is null)
        {
            throw new NotFoundException(DisplayTextFor.Movie, request.Id);
        }

        Console.WriteLine("test lalala");

        _context.MovieGenres.RemoveRange(movie.MovieGenres);

        foreach (var movieGenre in request.MovieGenres)
        {
            Console.WriteLine("test");
            var genreExits = await _context.Genres
            .Where(x => !x.IsDeleted && x.Id == movieGenre.GenreId)
            .AnyAsync(cancellationToken);

            if (!genreExits)
            {
                throw new NotFoundException(DisplayTextFor.Genre, movieGenre.GenreId);
            }

            _context.MovieGenres.Add(new MovieGenre
            {
                Id = Guid.NewGuid(),
                MovieId = movie.Id,
                GenreId = movieGenre.GenreId
            });
        }

        movie.Title = request.Title;
        movie.Rating = request.Rating;
        movie.Duration = request.Duration;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Synopsis = request.Synopsis;
        movie.PosterImage = request.PosterImage;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}

