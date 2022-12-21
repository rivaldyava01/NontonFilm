using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Studios.Commands.DeleteStudio;
using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Application.Studios.Commands.DeleteStudio;

[Authorize(Policy = Permissions.NontonFilm_Studio_Delete)]

public class DeleteStudioCommand : DeleteStudioRequest, IRequest
{
}

public class DeleteStudioCommandValidator : AbstractValidator<DeleteStudioCommand>
{
    public DeleteStudioCommandValidator()
    {
        Include(new DeleteStudioRequestValidator());
    }
}
public class DeleteStudioCommandHandler : IRequestHandler<DeleteStudioCommand>
{

    private readonly INontonFilmDbContext _context;

    public DeleteStudioCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteStudioCommand request, CancellationToken cancellationToken)
    {
        var studio = await _context.Studios
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.Cinema)
            .SingleOrDefaultAsync(cancellationToken);

        var seats = await _context.Seats
            .Where(x => !x.IsDeleted && x.StudioId == request.Id)
            .Include(a => a.Studio)
            .ToListAsync(cancellationToken);

        if (studio is null)
        {
            throw new NotFoundException(DisplayTextFor.Studio, request.Id);
        }

        foreach (var seat in seats)
        {
            seat.IsDeleted = true;
        }

        studio.IsDeleted = true;

        await _context.SaveChangesAsync(this, cancellationToken);
        return Unit.Value;
    }
}

