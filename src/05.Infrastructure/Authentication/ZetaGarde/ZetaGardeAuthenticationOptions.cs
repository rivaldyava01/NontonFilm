namespace Zeta.NontonFilm.Infrastructure.Authentication.ZetaGarde;

public class ZetaGardeAuthenticationOptions
{
    public static readonly string SectionKey = $"{nameof(Authentication)}:{nameof(ZetaGarde)}";

    public string AuthorityUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;
    public string ObjectId { get; set; } = default!;

    public string HealthCheckUrl => $"{AuthorityUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string HealthCheck { get; set; } = default!;
}
