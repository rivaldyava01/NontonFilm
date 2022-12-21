using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TokoPaEdi.Shared.Genres.Commands.DeleteGenre;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Genres.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Genres.Commands.DeleteGenre;


[Authorize(Policy = Permissions.NontonFilm_Genre_Delete)]

public class DeleteGenreCommand : DeleteGenreRequest, IRequest
{
}

public class DeleteGenreComanndValidator : AbstractValidator<DeleteGenreCommand>
{
    public DeleteGenreComanndValidator()
    {
        Include(new DeleteGenreRequestValidator());
    }
}

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
{
    private readonly INontonFilmDbContext _context;

    public DeleteGenreCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _context.Genres
       .Where(x => x.Id == request.Id)
       .Include(b => b.MovieGenres)
       .SingleOrDefaultAsync(cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(DisplayTextFor.Genre, request.Id);
        }

        if (genre.MovieGenres.Count > 0)
        {
            throw new ForbiddenAccessException("Cannot delete because already has movie");
        }

        genre.IsDeleted = true;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
