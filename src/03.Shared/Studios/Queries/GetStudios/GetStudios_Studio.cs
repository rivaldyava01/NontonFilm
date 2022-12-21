namespace Zeta.NontonFilm.Shared.Studios.Queries.GetStudios;

public class GetStudios_Studio
{
    public Guid Id { get; set; } = default!;
    public Guid CinemaId { get; set; } = default!;
    public string CinemaName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int TotalSeat { get; set; } = default!;
}
