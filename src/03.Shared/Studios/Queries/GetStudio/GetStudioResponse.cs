namespace Zeta.NontonFilm.Shared.Studios.Queries.GetStudio;

public class GetStudioResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid CinemaId { get; set; } = default!;
    public string CinemaName { get; set; } = default!;

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
