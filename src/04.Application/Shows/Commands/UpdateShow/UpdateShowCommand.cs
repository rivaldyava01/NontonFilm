using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Shows.Commands.UpdateShow;
using Zeta.NontonFilm.Shared.Shows.Constants;

namespace Zeta.NontonFilm.Application.Shows.Commands.UpdateShow;


[Authorize(Policy = Permissions.NontonFilm_Show_Edit)]

public class UpdateShowCommand : UpdateShowRequest, IRequest
{
}

public class UpdateShowCommandValidator : AbstractValidator<UpdateShowCommand>
{
    public UpdateShowCommandValidator()
    {
        Include(new UpdateShowRequestValidator());
    }
}

public class UpdateShowCommandHandler : IRequestHandler<UpdateShowCommand>
{
    private readonly INontonFilmDbContext _context;

    public UpdateShowCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateShowCommand request, CancellationToken cancellationToken)
    {
        var show = await _context.Shows
         .Where(x => !x.IsDeleted && x.Id == request.Id)
         .Include(a => a.Tickets)
         .SingleOrDefaultAsync(cancellationToken);

        if (show is null)
        {
            throw new NotFoundException(DisplayTextFor.Show, request.Id);
        }

        var showWithTheSameData = await _context.Shows
            .Where(x => !x.IsDeleted && x.MovieId == request.MovieId && x.StudioId == request.StudioId && x.ShowDateTime == request.ShowDateTime)
            .SingleOrDefaultAsync(cancellationToken);

        if (showWithTheSameData is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Show, DisplayTextFor.Name, request.StudioId);
        }

        show.MovieId = request.MovieId;
        show.ShowDateTime = request.ShowDateTime;
        show.TicketPrice = request.TicketPrice;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
