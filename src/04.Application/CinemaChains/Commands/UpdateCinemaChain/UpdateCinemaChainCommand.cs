using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.CinemaChains.Commands.UpdateCinemaChain;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.CinemaChains.Commands.UpdateCinemaChain;

[Authorize(Policy = Permissions.NontonFilm_CinemaChain_Edit)]

public class UpdateCinemaChainCommand : UpdateCinemaChainRequest, IRequest
{
}

public class UpdateCinemaChainCommandValidator : AbstractValidator<UpdateCinemaChainCommand>
{
    public UpdateCinemaChainCommandValidator()
    {
        Include(new UpdateCinemaChainRequestValidator());
    }
}

public class UpdateCinemaChainCommandHandler : IRequestHandler<UpdateCinemaChainCommand>
{
    private readonly INontonFilmDbContext _context;

    public UpdateCinemaChainCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCinemaChainCommand request, CancellationToken cancellationToken)
    {
        var cinemaChain = await _context.CinemaChains
         .Where(x => !x.IsDeleted && x.Id == request.Id)
         .SingleOrDefaultAsync(cancellationToken);

        if (cinemaChain is null)
        {
            throw new NotFoundException(DisplayTextFor.CinemaChain, request.Id);
        }

        var cinemaChainWithTheSameName = await _context.CinemaChains
            .Where(x => !x.IsDeleted && x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (cinemaChainWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.CinemaChain, DisplayTextFor.Name, request.Name);
        }

        cinemaChain.Name = request.Name;
        cinemaChain.OfficeAddress = request.OfficeAddress;
        cinemaChain.EmailAddress = request.EmailAddress;
        cinemaChain.PhoneNumber = request.PhoneNumber;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
