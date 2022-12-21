using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Movies.Commands.AddMovie;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Movies.Commands.AddMovie;

[Authorize(Policy = Permissions.NontonFilm_Movie_Add)]

public class AddMovieCommand : AddMovieRequest, IRequest<ItemCreatedResponse>
{
}

public class AddMovieCommandValidator : AbstractValidator<AddMovieCommand>
{
    public AddMovieCommandValidator()
    {
        Include(new AddMovieRequestValidator());
    }
}

public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;

    public AddMovieCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ItemCreatedResponse> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var movieWithTheSameTitle = await _context.Movies
            .Where(x => !x.IsDeleted && x.Title == request.Title)
            .SingleOrDefaultAsync(cancellationToken);

        if (movieWithTheSameTitle is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Movie, DisplayTextFor.Title, request.Title);
        }

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Rating = request.Rating,
            Duration = request.Duration,
            ReleaseDate = request.ReleaseDate,
            Synopsis = request.Synopsis,
            PosterImage = request.PosterImage,

        };

        foreach (var movieGenreId in request.MovieGenreIds)
        {
            var genreExits = await _context.Genres
            .Where(x => !x.IsDeleted && x.Id == movieGenreId)
            .AnyAsync(cancellationToken);

            if (!genreExits)
            {
                throw new NotFoundException(DisplayTextFor.Genre, movieGenreId);
            }

            movie.MovieGenres.Add(new MovieGenre
            {
                Id = Guid.NewGuid(),
                GenreId = movieGenreId
            });
        }

        await _context.Movies.AddAsync(movie, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = movie.Id
        };
    }
}
