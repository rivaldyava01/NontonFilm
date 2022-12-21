using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Cities.Constants;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Cities.Commands.AddCity;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cities.Commands.AddCity;

[Authorize(Policy = Permissions.NontonFilm_City_Add)]

public class AddCityCommand : AddCityRequest, IRequest<ItemCreatedResponse>
{
}

public class AddCityCommandValidator : AbstractValidator<AddCityCommand>
{
    public AddCityCommandValidator()
    {
        Include(new AddCityRequestValidator());
    }
}

public class AddCityCommandHandler : IRequestHandler<AddCityCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;

    public AddCityCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ItemCreatedResponse> Handle(AddCityCommand request, CancellationToken cancellationToken)
    {
        var cityWithTheSameName = await _context.Cities
            .Where(x => !x.IsDeleted && x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (cityWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.City, DisplayTextFor.Name, request.Name);
        }

        var city = new City
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        await _context.Cities.AddAsync(city, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = city.Id
        };
    }
}
