﻿namespace Zeta.NontonFilm.Infrastructure.Authorization.IS4IM;

public class IS4IMAuthorizationOptions
{
    public static readonly string SectionKey = $"{nameof(Authorization)}:{nameof(IS4IM)}";

    public string BaseUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;

    public string HealthCheckUrl => $"{BaseUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string HealthCheck { get; set; } = default!;
    public string AuthorizationInfo { get; set; } = default!;
}
