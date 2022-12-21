using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Movies.Commands.DeleteMovie;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Movies.Commands.DeleteMovie;

[Authorize(Policy = Permissions.NontonFilm_Movie_Delete)]

public class DeleteMovieCommand : DeleteMovieRequest, IRequest
{
}
public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        Include(new DeleteMovieRequestValidator());
    }
}
public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand>
{

    private readonly INontonFilmDbContext _context;

    public DeleteMovieCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.MovieGenres)
                .ThenInclude(b => b.Genre)
            .SingleOrDefaultAsync(cancellationToken);
        if (movie is null)
        {
            throw new NotFoundException(DisplayTextFor.Movie, request.Id);
        }

        _context.MovieGenres.RemoveRange(movie.MovieGenres);
        movie.IsDeleted = true;

        await _context.SaveChangesAsync(this, cancellationToken);
        return Unit.Value;
    }
}

