using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.CinemaChains.Commands.AddCinemaChain;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.CinemaChains.Commands.AddCinemaChain;

[Authorize(Policy = Permissions.NontonFilm_CinemaChain_Add)]

public class AddCinemaChainCommand : AddCinemaChainRequest, IRequest<ItemCreatedResponse>
{
}

public class AddCinemaChainCommandValidator : AbstractValidator<AddCinemaChainCommand>
{
    public AddCinemaChainCommandValidator()
    {
        Include(new AddCinemaChainRequestValidator());
    }
}

public class AddCinemaChainCommandHandler : IRequestHandler<AddCinemaChainCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;

    public AddCinemaChainCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ItemCreatedResponse> Handle(AddCinemaChainCommand request, CancellationToken cancellationToken)
    {
        var cinemaChainWithTheSameName = await _context.CinemaChains
            .Where(x => !x.IsDeleted && x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (cinemaChainWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.CinemaChain, DisplayTextFor.Name, request.Name);
        }

        var cinemaChain = new CinemaChain
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OfficeAddress = request.OfficeAddress,
            EmailAddress = request.EmailAddress,
            PhoneNumber = request.PhoneNumber
        };

        await _context.CinemaChains.AddAsync(cinemaChain, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = cinemaChain.Id
        };
    }
}
