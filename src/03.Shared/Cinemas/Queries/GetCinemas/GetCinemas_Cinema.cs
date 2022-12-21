namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemas;

public class GetCinemas_Cinema
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
}
