using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Email;
using Zeta.NontonFilm.Infrastructure.Email.None;
using Zeta.NontonFilm.Infrastructure.Email.SendGrid;
using Zeta.NontonFilm.Infrastructure.Email.Smtp;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionKey));

        var emailOptions = configuration.GetSection(EmailOptions.SectionKey).Get<EmailOptions>();
        var fluentEmailServicesBuilder = services.AddFluentEmail(emailOptions.SenderEmailAddress);

        switch (emailOptions.Provider)
        {
            case EmailProvider.None:
                services.AddNoneEmailService();
                break;
            case EmailProvider.Smtp:
                services.Configure<SmtpEmailOptions>(configuration.GetSection(SmtpEmailOptions.SectionKey));
                var smtpOptions = configuration.GetSection(SmtpEmailOptions.SectionKey).Get<SmtpEmailOptions>();
                fluentEmailServicesBuilder.AddSmtpEmailService(smtpOptions, healthChecksBuilder);
                break;
            case EmailProvider.SendGrid:
                services.Configure<SendGridEmailOptions>(configuration.GetSection(SendGridEmailOptions.SectionKey));
                var sendGridEmailOptions = configuration.GetSection(SendGridEmailOptions.SectionKey).Get<SendGridEmailOptions>();
                fluentEmailServicesBuilder.AddSendGridEmailService(sendGridEmailOptions, healthChecksBuilder);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Email)} {nameof(EmailOptions.Provider)}: {emailOptions.Provider}");
        }

        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
