using Zeta.NontonFilm.Base.Enums;

namespace Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovie;

public class GetNowShowingMovieResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public RatingTypes Rating { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; } = default!;
    public string PosterImage { get; set; } = default!;

    public List<GetNowShowingMovieResponse_Genre> MovieGenres { get; set; } = new();

}
