namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUserByMovieId;

public class GetCinemasForUserByMovieId_Studios
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal TicketPrice { get; set; } = default!;

    public List<GetCinemasForUserByMovieId_Shows> Shows { get; set; } = new();
}
