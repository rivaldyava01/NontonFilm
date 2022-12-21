using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Studios.Commands.AddStudio;
using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Application.Studios.Commands.AddStudio;

[Authorize(Policy = Permissions.NontonFilm_Studio_Add)]

public class AddStudioCommand : AddStudioRequest, IRequest<ItemCreatedResponse>
{
}

public class AddStudioCommandValidator : AbstractValidator<AddStudioCommand>
{
    public AddStudioCommandValidator()
    {
        Include(new AddStudioRequestValidator());
    }
}

public class AddStudioCommandHandler : IRequestHandler<AddStudioCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;

    public AddStudioCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ItemCreatedResponse> Handle(AddStudioCommand request, CancellationToken cancellationToken)
    {
        var studioWithTheSameName = await _context.Studios
            .Where(x => !x.IsDeleted && x.Name == request.Name && x.CinemaId == request.CinemaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (studioWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Studio, DisplayTextFor.Name, request.Name);
        }

        var studio = new Studio
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CinemaId = request.CinemaId
        };

        var alphabet = new char[26];

        for (var i = 0; i < request.Row; i++)
        {
            alphabet[i] = (char)(i + 'A');
            for (var j = 1; j <= request.Column; j++)
            {
                var seat = new Seat
                {
                    Id = Guid.NewGuid(),
                    StudioId = studio.Id,
                    Row = alphabet[i].ToString(),
                    Column = j.ToString(),
                };
                await _context.Seats.AddAsync(seat, cancellationToken);
            }
        }

        await _context.Studios.AddAsync(studio, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = studio.Id
        };
    }
}
