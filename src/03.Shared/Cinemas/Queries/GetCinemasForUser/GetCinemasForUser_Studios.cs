namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUser;

public class GetCinemasForUser_Studios
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string MovieName { get; set; } = default!;
    public decimal TicketPrice { get; set; } = default!;

    public List<GetCinemasForUser_Shows> Shows { get; set; } = new();
}
