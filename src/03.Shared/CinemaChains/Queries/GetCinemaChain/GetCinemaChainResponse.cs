namespace Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChain;
public class GetCinemaChainResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string OfficeAddress { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public List<GetCinemaChainResponse_Cinema> Cinemas { get; set; } = new();

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
