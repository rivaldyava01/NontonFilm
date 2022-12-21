using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketTransactionHistoriesBuUserid;

namespace Zeta.NontonFilm.Application.Tickets.Queries.GetTickets;

[Authorize(Policy = Permissions.NontonFilm_Ticket_User_Handle)]

public class GetTicketTransactionHistoriesByUserIdQuery : IRequest<ListResponse<GetTicketTransactionHistoriesbyUserIdResponse>>
{

}

public class GetTicketTransactionHistoriesByUserIdQueryHandler : IRequestHandler<GetTicketTransactionHistoriesByUserIdQuery, ListResponse<GetTicketTransactionHistoriesbyUserIdResponse>>
{
    private readonly INontonFilmDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTicketTransactionHistoriesByUserIdQueryHandler(INontonFilmDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<ListResponse<GetTicketTransactionHistoriesbyUserIdResponse>> Handle(GetTicketTransactionHistoriesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = new ListResponse<GetTicketTransactionHistoriesbyUserIdResponse>();

        var ticketsales = await _context.TicketSales
            .AsNoTracking()
            .Where(x => x.UserId == _currentUserService.UserId)
            .ToListAsync(cancellationToken);

        var tickets = await _context.Tickets
            .AsNoTracking()
            .Include(b => b.Show)
                .ThenInclude(c => c.Movie)
            .Where(x => ticketsales.Select(x => x.Id).Contains(x.TicketSalesId!.Value))
            .ToListAsync(cancellationToken);

        foreach (var ticketsale in ticketsales)
        {

            var ticketHistory = new GetTicketTransactionHistoriesbyUserIdResponse
            {
                TicketSalesId = ticketsale.Id,
                Created = ticketsale.SalesDateTime
            };

            var ticket = tickets
                .Where(x => x.TicketSalesId == ticketsale.Id)
                .FirstOrDefault();

            if (ticket is not null)
            {
                ticketHistory.MovieTitle = ticket.MovieName;
                ticketHistory.CinemaName = ticket.CinemaName;
                ticketHistory.DateShow = ticket.ShowDateTime.ToShortDateString();
                ticketHistory.TimeShow = ticket.ShowDateTime.ToShortTimeString();
                ticketHistory.MoviePosterImage = ticket.Show.Movie.PosterImage;
                ticketHistory.StudioName = ticket.StudioName;
                ticketHistory.SeatCode = string.Join(", ", tickets.Where(x => x.TicketSalesId == ticketsale.Id).Select(x => x.SeatCode));
            }

            response.Items.Add(ticketHistory);

        }

        return response;
    }
}
