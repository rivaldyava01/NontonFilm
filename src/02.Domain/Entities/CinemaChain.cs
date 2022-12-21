using Zeta.NontonFilm.Domain.Abstracts;

namespace Zeta.NontonFilm.Domain.Entities;

public class CinemaChain : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string OfficeAddress { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public List<Cinema> Cinemas { get; set; } = new();
}
