﻿using Zeta.NontonFilm.Bsui.Services.Telemetry.ApplicationInsights;
using Zeta.NontonFilm.Bsui.Services.Telemetry.None;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Bsui.Services.Telemetry;

public static class DependencyInjection
{
    public static IServiceCollection AddTelemetryService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelemetryOptions>(configuration.GetSection(TelemetryOptions.SectionKey));

        var telemetryOptions = configuration.GetSection(TelemetryOptions.SectionKey).Get<TelemetryOptions>();

        switch (telemetryOptions.Provider)
        {
            case TelemetryProvider.None:
                services.AddNoneTelemetryService();
                break;
            case TelemetryProvider.ApplicationInsights:
                services.AddApplicationInsightsTelemetryService(configuration);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Telemetry)} {nameof(TelemetryOptions.Provider)}: {telemetryOptions.Provider}");
        }

        return services;
    }
}
