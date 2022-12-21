namespace Zeta.NontonFilm.Shared.Cities.Queries.GetCity;

public class GetCityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public List<GetCityResponse_Cinema> Cinemas { get; set; } = new();

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
