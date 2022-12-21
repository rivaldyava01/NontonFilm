namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUser;

public class GetCinemasForUser_Cinema
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public List<GetCinemasForUser_Studios> Studios { get; set; } = new();
}
