using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Genres.Constants;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Genres.Commands.AddGenre;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Genres.Commands.AddGenre;

[Authorize(Policy = Permissions.NontonFilm_Genre_Add)]

public class AddGenreCommand : AddGenreRequest, IRequest<ItemCreatedResponse>
{
}

public class AddGenreCommandValidator : AbstractValidator<AddGenreCommand>
{
    public AddGenreCommandValidator()
    {
        Include(new AddGenreRequestValidator());
    }
}

public class AddGenreCommandHandler : IRequestHandler<AddGenreCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;

    public AddGenreCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ItemCreatedResponse> Handle(AddGenreCommand request, CancellationToken cancellationToken)
    {
        var genreWithTheSameName = await _context.Genres
            .Where(x => !x.IsDeleted && x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (genreWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Genre, DisplayTextFor.Name, request.Name);
        }

        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        await _context.Genres.AddAsync(genre, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = genre.Id
        };
    }
}
