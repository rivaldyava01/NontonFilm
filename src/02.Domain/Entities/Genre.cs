using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class Genre : AuditableEntity
{
    public string Name { get; set; } = default!;

    public List<MovieGenre> MovieGenres { get; set; } = new();
}
