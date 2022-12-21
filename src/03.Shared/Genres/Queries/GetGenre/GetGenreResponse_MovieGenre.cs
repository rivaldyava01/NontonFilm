namespace Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;

public class GetGenreResponse_MovieGenre
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public string MoviePosterImage { get; set; } = default!;
}
