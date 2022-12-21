using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Application.Services.Sms;
using Zeta.NontonFilm.Application.Services.Sms.Models.SendSms;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Sms.None;

public class NoneSmsService : ISmsService
{
    private readonly ILogger<NoneSmsService> _logger;

    public NoneSmsService(ILogger<NoneSmsService> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(Sms).ToUpper()} {CommonDisplayTextFor.Service}");
    }

    public Task SendSmsAsync(SendSmsRequest smsModel)
    {
        LogWarning();

        return Task.CompletedTask;
    }
}
