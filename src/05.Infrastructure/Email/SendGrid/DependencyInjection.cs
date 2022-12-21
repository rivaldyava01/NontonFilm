using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Email.SendGrid;

public static class DependencyInjection
{
    public static FluentEmailServicesBuilder AddSendGridEmailService(this FluentEmailServicesBuilder fluentEmailServicesBuilder, SendGridEmailOptions sendGridEmailOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        fluentEmailServicesBuilder.AddSendGridSender(sendGridEmailOptions.ApiKey);

        healthChecksBuilder.AddSendGrid(
            apiKey: sendGridEmailOptions.ApiKey,
            name: $"{nameof(Email)} {CommonDisplayTextFor.Service} ({nameof(SendGrid)})",
            failureStatus: HealthStatus.Degraded);

        return fluentEmailServicesBuilder;
    }
}
