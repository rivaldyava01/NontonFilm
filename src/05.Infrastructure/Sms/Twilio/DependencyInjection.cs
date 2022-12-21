using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Application.Services.Sms;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Sms.Twilio;

public static class DependencyInjection
{
    public static IServiceCollection AddTwilioSmsService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<TwilioSmsOptions>(configuration.GetSection(TwilioSmsOptions.SectionKey));
        services.AddSingleton<ISmsService, TwilioSmsService>();

        var twilioSmsOptions = configuration.GetSection(TwilioSmsOptions.SectionKey).Get<TwilioSmsOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Sms).ToUpper()} {CommonDisplayTextFor.Service} ({nameof(Twilio)})",
            instance: new TwilioSmsHealthCheck(twilioSmsOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Degraded,
            tags: default));

        return services;
    }
}
