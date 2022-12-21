using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Cities.Commands.UpdateCity;
using Zeta.NontonFilm.Shared.Cities.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cities.Commands.UpdateCity;

[Authorize(Policy = Permissions.NontonFilm_City_Edit)]

public class UpdateCityCommand : UpdateCityRequest, IRequest
{
}

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        Include(new UpdateCityRequestValidator());
    }
}

public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
{
    private readonly INontonFilmDbContext _context;

    public UpdateCityCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _context.Cities
         .Where(x => !x.IsDeleted && x.Id == request.Id)
         .SingleOrDefaultAsync(cancellationToken);

        if (city is null)
        {
            throw new NotFoundException(DisplayTextFor.City, request.Id);
        }

        var cityWithTheSameName = await _context.Cities
            .Where(x => !x.IsDeleted && x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (cityWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.City, DisplayTextFor.Name, request.Name);
        }

        city.Name = request.Name;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
