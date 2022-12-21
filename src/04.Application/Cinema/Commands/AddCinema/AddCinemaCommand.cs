using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Cinemas.Commands.AddCinema;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cinemas.Commands.AddCinema;

[Authorize(Policy = Permissions.NontonFilm_Cinema_Add)]

public class AddCinemaCommand : AddCinemaRequest, IRequest<ItemCreatedResponse>
{
}

public class AddCinemaCommandValidator : AbstractValidator<AddCinemaCommand>
{
    public AddCinemaCommandValidator()
    {
        Include(new AddCinemaRequestValidator());
    }
}

public class AddCinemaCommandHandler : IRequestHandler<AddCinemaCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;

    public AddCinemaCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ItemCreatedResponse> Handle(AddCinemaCommand request, CancellationToken cancellationToken)
    {
        var movieWithTheSameTitle = await _context.Cinemas
            .Where(x => !x.IsDeleted && x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (movieWithTheSameTitle is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Cinema, DisplayTextFor.Name, request.Name);
        }

        var movie = new Cinema
        {
            Id = Guid.NewGuid(),
            CityId = request.CityId,
            CinemaChainId = request.CinemaChainId,
            Name = request.Name,
            Address = request.Address,
            EmailAddress = request.EmailAddress,
            PhoneNumber = request.PhoneNumber,
        };

        await _context.Cinemas.AddAsync(movie, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = movie.Id
        };
    }
}
