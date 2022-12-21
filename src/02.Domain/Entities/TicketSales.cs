using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class TicketSales : Entity
{
    public Guid UserId { get; set; }
    public DateTime SalesDateTime { get; set; }

    public List<Ticket> Tickets { get; set; } = new();
}
