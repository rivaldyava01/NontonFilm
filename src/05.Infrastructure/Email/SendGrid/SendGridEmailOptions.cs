namespace Zeta.NontonFilm.Infrastructure.Email.SendGrid;

public class SendGridEmailOptions
{
    public static readonly string SectionKey = $"{nameof(Email)}:{nameof(SendGrid)}";

    public string ApiKey { get; set; } = default!;
}
