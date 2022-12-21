using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class Seat : AuditableEntity
{
    public Studio Studio { get; set; } = default!;
    public Guid StudioId { get; set; }

    public string Column { get; set; } = default!;
    public string Row { get; set; } = default!;

    public List<Ticket> Tickets { get; set; } = new();
}
