using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Application.Services.UserProfile;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Common.Extensions;

namespace Zeta.NontonFilm.Infrastructure.UserProfile.ZetaGarde;

public static class DependencyInjection
{
    public static IServiceCollection AddZetaGardeUserProfileService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<ZetaGardeUserProfileOptions>(configuration.GetSection(ZetaGardeUserProfileOptions.SectionKey));
        services.AddTransient<IUserProfileService, ZetaGardeUserProfileService>();

        var zetaGardeUserProfileOptions = configuration.GetSection(ZetaGardeUserProfileOptions.SectionKey).Get<ZetaGardeUserProfileOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(UserProfile).SplitWords()} {CommonDisplayTextFor.Service} ({nameof(ZetaGarde)})",
            instance: new ZetaGardeUserProfileHealthCheck(zetaGardeUserProfileOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
