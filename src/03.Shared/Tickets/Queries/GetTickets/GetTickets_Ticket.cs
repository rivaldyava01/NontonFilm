namespace Zeta.NontonFilm.Shared.Tickets.Queries.GetTickets;

public class GetTickets_Ticket
{
    public Guid ShowId { get; set; }
    public Guid? TicketSalesId { get; set; }
    public Guid SeatId { get; set; }

    public string CinemaName { get; set; } = default!;
    public string StudioName { get; set; } = default!;
    public string SeatCode { get; set; } = default!;
    public string MovieName { get; set; } = default!;
    public decimal TicketPrice { get; set; }
    public DateTime ShowDateTime { get; set; }
}
