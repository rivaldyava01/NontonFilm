using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Application.Services.Email;
using Zeta.NontonFilm.Application.Services.Email.Models.SendEmail;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Email.None;

public class NoneEmailService : IEmailService
{
    private readonly ILogger<NoneEmailService> _logger;

    public NoneEmailService(ILogger<NoneEmailService> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(Email)} {CommonDisplayTextFor.Service}");
    }

    public Task SendEmailAsync(SendEmailRequest emailModel)
    {
        LogWarning();

        return Task.CompletedTask;
    }
}
