using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class Ticket : Entity
{
    public Show Show { get; set; } = default!;
    public Guid ShowId { get; set; }

    public TicketSales? TicketSales { get; set; } = default!;
    public Guid? TicketSalesId { get; set; }

    public Seat Seat { get; set; } = default!;
    public Guid SeatId { get; set; }

    public string CinemaName { get; set; } = default!;
    public string StudioName { get; set; } = default!;
    public string SeatCode { get; set; } = default!;
    public string MovieName { get; set; } = default!;
    public decimal TicketPrice { get; set; }
    public DateTime ShowDateTime { get; set; }
}
