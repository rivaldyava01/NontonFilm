using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Seats.Constants;
using Zeta.NontonFilm.Shared.Seats.Queries.GetSeats;

namespace Zeta.NontonFilm.Application.Seats.Queries.GetSeats;

public class GetSeatsQuery : IRequest<GetSeatsResponse>
{
    public Guid StudioId { get; set; }
}

public class GetSeatsQueryHandler : IRequestHandler<GetSeatsQuery, GetSeatsResponse>
{
    private readonly INontonFilmDbContext _context;

    public GetSeatsQueryHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<GetSeatsResponse> Handle(GetSeatsQuery request, CancellationToken cancellationToken)
    {
        var row = await _context.Seats
         .Where(x => !x.IsDeleted && x.StudioId == request.StudioId)
         .Select(x => x.Row)
         .Distinct()
         .ToListAsync(cancellationToken);

        if (row is null)
        {
            throw new NotFoundException(DisplayTextFor.Studio, request.StudioId);
        }

        var column = await _context.Seats
         .Where(x => !x.IsDeleted && x.StudioId == request.StudioId)
         .Select(x => x.Column)
         .Distinct()
         .ToListAsync(cancellationToken);

        if (column is null)
        {
            throw new NotFoundException(DisplayTextFor.Studio, request.StudioId);
        }

        return new GetSeatsResponse
        {
            Row = row.Count(),
            Column = column.Count()
        };
    }
}
