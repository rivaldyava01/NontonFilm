using Zeta.NontonFilm.Application.Services.Sms.Models.SendSms;

namespace Zeta.NontonFilm.Application.Services.Sms;

public interface ISmsService
{
    Task SendSmsAsync(SendSmsRequest sendSmsRequest);
}
