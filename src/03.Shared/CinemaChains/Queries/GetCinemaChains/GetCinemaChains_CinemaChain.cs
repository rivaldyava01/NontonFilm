namespace Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChains;

public class GetCinemaChains_CinemaChain
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string OfficeAddress { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

}
