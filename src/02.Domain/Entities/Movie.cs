using Zeta.NontonFilm.Base.Enums;
using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class Movie : AuditableEntity
{
    public string Title { get; set; } = default!;
    public RatingTypes Rating { get; set; } = default!;
    public int Duration { get; set; } = default!;
    public DateTime ReleaseDate { get; set; } = default!;
    public string Synopsis { get; set; } = default!;
    public string PosterImage { get; set; } = default!;

    public List<MovieGenre> MovieGenres { get; set; } = new();
    public List<Show> Shows { get; set; } = new();
}
