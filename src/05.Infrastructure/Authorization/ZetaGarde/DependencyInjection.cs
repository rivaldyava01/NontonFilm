using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Application.Services.Authorization;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Authorization.ZetaGarde;

public static class DependencyInjection
{
    public static IServiceCollection AddZetaGardeAuthorizationService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<ZetaGardeAuthorizationOptions>(configuration.GetSection(ZetaGardeAuthorizationOptions.SectionKey));
        services.AddTransient<IAuthorizationService, ZetaGardeAuthorizationService>();

        var zetaGardeAuthorizationOptions = configuration.GetSection(ZetaGardeAuthorizationOptions.SectionKey).Get<ZetaGardeAuthorizationOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Authorization)} {CommonDisplayTextFor.Service} ({nameof(ZetaGarde)})",
            instance: new ZetaGardeAuthorizationHealthCheck(zetaGardeAuthorizationOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
