using Zeta.NontonFilm.Application.Services.Email.Models.SendEmail;

namespace Zeta.NontonFilm.Application.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailRequest sendEmailRequest);
}
