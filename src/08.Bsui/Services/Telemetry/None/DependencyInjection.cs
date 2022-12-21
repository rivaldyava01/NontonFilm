using Zeta.NontonFilm.Bsui.Services.Logging;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Bsui.Services.Telemetry.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneTelemetryService(this IServiceCollection services)
    {
        LoggingHelper
            .CreateLogger()
            .LogWarning("{ServiceName} is set to None.", $"{nameof(Telemetry)} {CommonDisplayTextFor.Service}");

        return services;
    }
}
