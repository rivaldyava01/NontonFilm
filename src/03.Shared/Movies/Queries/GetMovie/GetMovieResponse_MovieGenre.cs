namespace Zeta.NontonFilm.Shared.Movies.Queries.GetMovie;

public class GetMovieResponse_MovieGenre
{
    public Guid Id { get; set; }
    public Guid GenreId { get; set; }
    public string GenreName { get; set; } = default!;
}
