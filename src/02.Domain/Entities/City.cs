using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class City : AuditableEntity
{
    public string Name { get; set; } = default!;

    public List<Cinema> Cinemas { get; set; } = new();
}
