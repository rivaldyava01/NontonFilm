using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Cinemas.Commands.DeleteCinema;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cinemas.Commands.DeleteCinema;

[Authorize(Policy = Permissions.NontonFilm_Cinema_Delete)]

public class DeleteCinemaCommand : DeleteCinemaRequest, IRequest
{
}

public class DeleteCinemaComanndValidator : AbstractValidator<DeleteCinemaCommand>
{
    public DeleteCinemaComanndValidator()
    {
        Include(new DeleteCinemaRequestValidator());
    }
}

public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaCommand>
{
    private readonly INontonFilmDbContext _context;

    public DeleteCinemaCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCinemaCommand request, CancellationToken cancellationToken)
    {
        var cinema = await _context.Cinemas
       .Where(x => !x.IsDeleted && x.Id == request.Id)
       .SingleOrDefaultAsync(cancellationToken);

        if (cinema is null)
        {
            throw new NotFoundException(DisplayTextFor.Cinema, request.Id);
        }

        if (cinema.Studios is not null)
        {
            throw new RelatedAnotherDatasException(nameof(cinema), request.Id);
        }

        cinema.IsDeleted = true;

        await _context.SaveChangesAsync(this, cancellationToken);

        return Unit.Value;
    }
}
