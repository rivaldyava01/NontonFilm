using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class Show : AuditableEntity
{
    public Studio Studio { get; set; } = default!;
    public Guid StudioId { get; set; }

    public Movie Movie { get; set; } = default!;
    public Guid MovieId { get; set; }

    public DateTime ShowDateTime { get; set; }
    public decimal TicketPrice { get; set; }

    public List<Ticket> Tickets { get; set; } = new();
}
