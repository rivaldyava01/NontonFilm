using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TokoPaEdi.Shared.Shows.Commands.DeleteShow;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Shows.Constants;

namespace Zeta.NontonFilm.Application.Shows.Commands.DeleteShow;

[Authorize(Policy = Permissions.NontonFilm_Show_Delete)]

public class DeleteShowCommand : DeleteShowRequest, IRequest
{
}

public class DeleteShowComanndValidator : AbstractValidator<DeleteShowCommand>
{
    public DeleteShowComanndValidator()
    {
        Include(new DeleteShowRequestValidator());
    }
}

public class DeleteShowCommandHandler : IRequestHandler<DeleteShowCommand>
{
    private readonly INontonFilmDbContext _context;

    public DeleteShowCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteShowCommand request, CancellationToken cancellationToken)
    {
        var show = await _context.Shows
       .Where(x => x.Id == request.Id)
       .Include(a => a.Movie)
       .Include(b => b.Studio)
       .Include(c => c.Tickets)
       .SingleOrDefaultAsync(cancellationToken);

        if (show is null)
        {
            throw new NotFoundException(DisplayTextFor.Show, request.Id);
        }

        foreach (var ticket in show.Tickets)
        {
            if (ticket.TicketSales is not null)
            {
                throw new RelatedAnotherDatasException(nameof(ticket), request.Id);
            }
        }

        show.IsDeleted = true;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
