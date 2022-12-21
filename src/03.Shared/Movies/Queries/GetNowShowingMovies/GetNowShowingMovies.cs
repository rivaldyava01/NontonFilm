namespace Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovies;

public class GetNowShowingMovies_Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string PosterImage { get; set; } = default!;
}
