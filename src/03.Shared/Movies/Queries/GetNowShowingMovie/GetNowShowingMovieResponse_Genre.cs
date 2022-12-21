namespace Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovie;

public class GetNowShowingMovieResponse_Genre
{
    public Guid Id { get; set; }
    public Guid GenreId { get; set; }
    public string GenreName { get; set; } = default!;
}
