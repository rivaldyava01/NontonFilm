using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TokoPaEdi.Shared.CinemaChains.Commands.DeleteCinemaChain;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.CinemaChains.Commands.DeleteCinemaChain;

[Authorize(Policy = Permissions.NontonFilm_CinemaChain_Delete)]

public class DeleteCinemaChainCommand : DeleteCinemaChainRequest, IRequest
{
}

public class DeleteCinemaChainComanndValidator : AbstractValidator<DeleteCinemaChainCommand>
{
    public DeleteCinemaChainComanndValidator()
    {
        Include(new DeleteCinemaChainRequestValidator());
    }
}

public class DeleteCinemaChainCommandHandler : IRequestHandler<DeleteCinemaChainCommand>
{
    private readonly INontonFilmDbContext _context;

    public DeleteCinemaChainCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCinemaChainCommand request, CancellationToken cancellationToken)
    {
        var cinemaChain = await _context.CinemaChains
       .Where(x => !x.IsDeleted && x.Id == request.Id)
       .SingleOrDefaultAsync(cancellationToken);

        if (cinemaChain is null)
        {
            throw new NotFoundException(DisplayTextFor.CinemaChain, request.Id);
        }

        if (cinemaChain.Cinemas is not null)
        {
            throw new RelatedAnotherDatasException(nameof(cinemaChain), request.Id);
        }

        cinemaChain.IsDeleted = true;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
