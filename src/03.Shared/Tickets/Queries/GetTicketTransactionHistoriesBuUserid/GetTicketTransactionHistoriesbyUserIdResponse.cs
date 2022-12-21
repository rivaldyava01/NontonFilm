namespace Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketTransactionHistoriesBuUserid;
public class GetTicketTransactionHistoriesbyUserIdResponse
{
    public Guid TicketSalesId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public string MoviePosterImage { get; set; } = default!;
    public string CinemaName { get; set; } = default!;
    public string DateShow { get; set; } = default!;
    public string TimeShow { get; set; } = default!;
    public string SeatCode { get; set; } = default!;
    public string StudioName { get; set; } = default!;
    public DateTimeOffset Created { get; set; }
}
