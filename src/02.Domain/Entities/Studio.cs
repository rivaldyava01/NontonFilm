using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class Studio : AuditableEntity
{
    public Cinema Cinema { get; set; } = default!;
    public Guid CinemaId { get; set; }

    public string Name { get; set; } = default!;

    public List<Seat> Seats { get; set; } = new();
    public List<Show> Shows { get; set; } = new();
}
