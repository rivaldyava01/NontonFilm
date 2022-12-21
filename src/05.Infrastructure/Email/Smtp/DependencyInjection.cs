using HealthChecks.Network.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Infrastructure.Email.FluentMailkit;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Email.Smtp;

public static class DependencyInjection
{
    public static FluentEmailServicesBuilder AddSmtpEmailService(this FluentEmailServicesBuilder fluentEmailServicesBuilder, SmtpEmailOptions smtpEmailOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        var fluentSmtpClientOptions = new FluentSmtpClientOptions
        {
            Server = smtpEmailOptions.Host,
            Port = smtpEmailOptions.Port,
            User = smtpEmailOptions.Username,
            Password = smtpEmailOptions.Password,
            UseSsl = smtpEmailOptions.EnableSsl,
            SocketOptions = MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable,
            RequiresAuthentication = true
        };

        fluentEmailServicesBuilder.AddFluentMailKitSender(fluentSmtpClientOptions);

        healthChecksBuilder.AddSmtpHealthCheck(
            options =>
            {
                options.Host = smtpEmailOptions.Host;
                options.Port = smtpEmailOptions.Port;
                options.ConnectionType = SmtpConnectionType.PLAIN;
                options.LoginWith(smtpEmailOptions.Username, smtpEmailOptions.Password);
                options.AllowInvalidRemoteCertificates = true;
            },
            name: $"{nameof(Email)} {CommonDisplayTextFor.Service} ({smtpEmailOptions.Host}:{smtpEmailOptions.Port})",
            failureStatus: HealthStatus.Degraded);

        return fluentEmailServicesBuilder;
    }
}
