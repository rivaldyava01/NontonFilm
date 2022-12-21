namespace Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;

public class GetGenreResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public List<GetGenreResponse_MovieGenre> MovieGenres { get; set; } = new();

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
