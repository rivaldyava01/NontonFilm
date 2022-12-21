using Zeta.NontonFilm.Base.Enums;

namespace Zeta.NontonFilm.Shared.Movies.Queries.GetMovie;

public class GetMovieResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public RatingTypes Rating { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; } = default!;
    public string PosterImage { get; set; } = default!;

    public List<GetMovieResponse_MovieGenre> MovieGenres { get; set; } = new();

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
