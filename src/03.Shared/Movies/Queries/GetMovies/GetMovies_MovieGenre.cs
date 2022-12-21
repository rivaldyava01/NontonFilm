namespace Zeta.NontonFilm.Shared.Movies.Queries.GetMovies;

public class GetMovies_MovieGenre
{
    public Guid Id { get; set; }
    public Guid GenreId { get; set; }
    public string GenreName { get; set; } = default!;
}
