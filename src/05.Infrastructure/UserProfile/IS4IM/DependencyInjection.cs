using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Application.Services.UserProfile;
using Zeta.NontonFilm.Shared.Common.Extensions;

namespace Zeta.NontonFilm.Infrastructure.UserProfile.IS4IM;

public static class DependencyInjection
{
    public static IServiceCollection AddIS4IMUserProfileService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<IS4IMUserProfileOptions>(configuration.GetSection(IS4IMUserProfileOptions.SectionKey));
        services.AddTransient<IUserProfileService, IS4IMUserProfileService>();

        var is4imUserProfileOptions = configuration.GetSection(IS4IMUserProfileOptions.SectionKey).Get<IS4IMUserProfileOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(UserProfile).SplitWords()} Service ({nameof(IS4IM)})",
            instance: new IS4IMUserProfileHealthCheck(is4imUserProfileOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
