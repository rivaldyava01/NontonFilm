using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Seats.Queries.GetSeat;

namespace Zeta.NontonFilm.Application.Seats.Queries.GetSeat;
public class GetSeatQuery : GetSeatRequest, IRequest<GetSeatResponse>
{
}

public class GetSeatQueryValidator : AbstractValidator<GetSeatQuery>
{
    public GetSeatQueryValidator()
    {
        Include(new GetSeatRequestValidator());
    }
}

public class GetSeatQueryHandler : IRequestHandler<GetSeatQuery, GetSeatResponse>
{
    private readonly INontonFilmDbContext _context;

    public GetSeatQueryHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<GetSeatResponse> Handle(GetSeatQuery request, CancellationToken cancellationToken)
    {
        var seat = await _context.Seats
            .Where(x => !x.IsDeleted && x.StudioId == request.StudioId && x.Column == request.Column && x.Row == request.Row)
            .SingleOrDefaultAsync(cancellationToken);

        if (seat is null)
        {
            throw new NotFoundException();
        }

        return new GetSeatResponse
        {
            Id = seat.Id
        };
    }
}
