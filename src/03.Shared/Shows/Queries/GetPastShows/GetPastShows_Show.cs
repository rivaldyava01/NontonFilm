namespace Zeta.NontonFilm.Shared.Shows.Queries.GetPastShows;

public class GetPastShows_Show
{
    public Guid Id { get; set; } = default!;
    public Guid MovieId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public Guid StudioId { get; set; }
    public string StudioName { get; set; } = default!;
    public DateTime ShowDateTime { get; set; }
    public decimal TicketPrice { get; set; }
}
