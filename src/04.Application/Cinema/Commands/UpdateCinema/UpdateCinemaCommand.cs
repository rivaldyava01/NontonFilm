using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Cinemas.Commands.UpdateCinema;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cinemas.Commands.UpdateCinema;

[Authorize(Policy = Permissions.NontonFilm_Cinema_Edit)]

public class UpdateCinemaCommand : UpdateCinemaRequest, IRequest
{
}

public class UpdateCinemaCommandValidator : AbstractValidator<UpdateCinemaCommand>
{
    public UpdateCinemaCommandValidator()
    {
        Include(new UpdateCinemaRequestValidator());
    }
}

public class UpdateCinemaCommandHandler : IRequestHandler<UpdateCinemaCommand>
{
    private readonly INontonFilmDbContext _context;

    public UpdateCinemaCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCinemaCommand request, CancellationToken cancellationToken)
    {
        var city = await _context.Cinemas
         .Where(x => !x.IsDeleted && x.Id == request.Id)
         .SingleOrDefaultAsync(cancellationToken);

        if (city is null)
        {
            throw new NotFoundException(DisplayTextFor.Cinema, request.Id);
        }

        var cityWithTheSameName = await _context.Cinemas
            .Where(x => !x.IsDeleted && x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (cityWithTheSameName is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Cinema, DisplayTextFor.Name, request.Name);
        }

        city.Name = request.Name;
        city.CinemaChainId = request.CinemaChainId;
        city.CityId = request.CityId;
        city.Address = request.Address;
        city.EmailAddress = request.EmailAddress;
        city.PhoneNumber = request.PhoneNumber;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
