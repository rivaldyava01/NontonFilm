namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUserByMovieId;

public class GetCinemasForUserByMovieId_Cinema
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public List<GetCinemasForUserByMovieId_Studios> Studios { get; set; } = new();
}
