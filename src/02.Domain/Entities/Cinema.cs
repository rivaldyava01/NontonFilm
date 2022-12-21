using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class Cinema : AuditableEntity
{
    public CinemaChain CinemaChain { get; set; } = default!;
    public Guid CinemaChainId { get; set; }

    public City City { get; set; } = default!;
    public Guid CityId { get; set; }

    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public List<Studio> Studios { get; set; } = new();

}
