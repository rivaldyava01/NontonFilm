using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class MovieGenre : Entity
{
    public Movie Movie { get; set; } = default!;
    public Guid MovieId { get; set; }

    public Genre Genre { get; set; } = default!;
    public Guid GenreId { get; set; }
}
