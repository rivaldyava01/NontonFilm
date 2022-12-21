using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Genres.Commands.UpdateGenre;
using Zeta.NontonFilm.Shared.Genres.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Genres.Commands.UpdateGenre;

[Authorize(Policy = Permissions.NontonFilm_Genre_Edit)]

public class UpdateGenreCommand : UpdateGenreRequest, IRequest
{
}

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        Include(new UpdateGenreRequestValidator());
    }
}

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand>
{
    private readonly INontonFilmDbContext _context;

    public UpdateGenreCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _context.Genres
         .Where(x => !x.IsDeleted && x.Id == request.Id)
         .SingleOrDefaultAsync(cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(DisplayTextFor.Genre, request.Id);
        }

        if (genre.Name != request.Name)
        {
            var genreWithTheSameName = await _context.Genres
         .Where(x => !x.IsDeleted && x.Name == request.Name)
         .SingleOrDefaultAsync(cancellationToken);

            if (genreWithTheSameName is not null)
            {
                throw new AlreadyExistsException(DisplayTextFor.Genre, DisplayTextFor.Name, request.Name);
            }

        }

        genre.Name = request.Name;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
