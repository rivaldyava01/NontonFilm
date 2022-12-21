using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTickets;

namespace Zeta.NontonFilm.Application.Tickets.Queries.GetTickets;

[Authorize(Policy = Permissions.NontonFilm_Ticket_User_Handle)]

public class GetTicketsQuery : GetTicketsRequest, IRequest<ListResponse<GetTickets_Ticket>>
{

}

public class GetTicketMapping : IMapFrom<Ticket, GetTickets_Ticket>
{
}

public class GetTicketQueryHandler : IRequestHandler<GetTicketsQuery, ListResponse<GetTickets_Ticket>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetTicketQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetTickets_Ticket>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = _context.Tickets
            .Where(x => x.ShowId == request.ShowId);

        var result = await tickets
        .ProjectTo<GetTickets_Ticket>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
        return result.ToListResponse();
    }
}
