using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Studios.Commands.UpdateStudio;
using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Application.Studios.Commands.UpdateStudio;

[Authorize(Policy = Permissions.NontonFilm_Studio_Delete)]

public class UpdateStudioCommand : UpdateStudioRequest, IRequest
{
}

public class UpdateStudioCommandValidator : AbstractValidator<UpdateStudioCommand>
{
    public UpdateStudioCommandValidator()
    {
        Include(new UpdateStudioRequestValidator());
    }
}

public class UpdateStudioCommandHandler : IRequestHandler<UpdateStudioCommand>
{
    private readonly INontonFilmDbContext _context;

    public UpdateStudioCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateStudioCommand request, CancellationToken cancellationToken)
    {
        var studio = await _context.Studios
         .Where(x => !x.IsDeleted && x.Id == request.Id)
         .SingleOrDefaultAsync(cancellationToken);

        if (studio is null)
        {
            throw new NotFoundException(DisplayTextFor.Studio, request.Id);
        }

        var studioWithTheSameName = await _context.Studios
            .Where(x => !x.IsDeleted && x.Name == request.Name && x.CinemaId == request.CinemaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (studioWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Studio, DisplayTextFor.Name, request.Name);
        }

        var seats = await _context.Seats
         .Where(x => !x.IsDeleted && x.StudioId == request.Id)
         .ToListAsync(cancellationToken);

        studio.Name = request.Name;
        studio.CinemaId = request.CinemaId;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
