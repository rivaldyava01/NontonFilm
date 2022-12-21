namespace Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde;

public class ZetaGardeAuthorizationOptions
{
    public static readonly string SectionKey = $"{nameof(Authorization)}:{nameof(ZetaGarde)}";

    public string BaseUrl { get; set; } = default!;
    public string ObjectId { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;

    public string HealthCheckUrl => $"{BaseUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string HealthCheck { get; set; } = default!;
    public string Positions { get; set; } = default!;
    public string AuthorizationInfo { get; set; } = default!;
}
