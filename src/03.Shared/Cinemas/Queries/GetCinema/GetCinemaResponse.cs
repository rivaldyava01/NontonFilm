namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinema;

public class GetCinemaResponse
{
    public Guid Id { get; set; } = default!;
    public Guid CityId { get; set; } = default!;
    public string CityName { get; set; } = default!;
    public Guid CinemaChainId { get; set; } = default!;
    public string CinemaChainName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public List<GetCinemaResponse_Studio> Studios { get; set; } = new();

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
