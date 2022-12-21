using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TokoPaEdi.Shared.Cities.Commands.DeleteCity;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Cities.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cities.Commands.DeleteCity;

[Authorize(Policy = Permissions.NontonFilm_City_Delete)]

public class DeleteCityCommand : DeleteCityRequest, IRequest
{
}

public class DeleteCityComanndValidator : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityComanndValidator()
    {
        Include(new DeleteCityRequestValidator());
    }
}

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly INontonFilmDbContext _context;

    public DeleteCityCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _context.Cities
       .Where(x => !x.IsDeleted && x.Id == request.Id)
       .SingleOrDefaultAsync(cancellationToken);

        if (city is null)
        {
            throw new NotFoundException(DisplayTextFor.City, request.Id);
        }

        if (city.Cinemas is not null)
        {
            throw new RelatedAnotherDatasException(nameof(City), request.Id);
        }

        city.IsDeleted = true;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
