using Zeta.NontonFilm.Base.Enums;

namespace Zeta.NontonFilm.Shared.Movies.Queries.GetMovies;

public class GetMovies_Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public RatingTypes Rating { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; } = default!;
    public string PosterImage { get; set; } = default!;

    public List<GetMovies_MovieGenre> MovieGenres { get; set; } = new();
}
