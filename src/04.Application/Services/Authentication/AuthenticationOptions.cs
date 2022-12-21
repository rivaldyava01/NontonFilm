using Zeta.NontonFilm.Shared.Services.Authentication.Constants;

namespace Zeta.NontonFilm.Application.Services.Authentication;

public class AuthenticationOptions
{
    public const string SectionKey = nameof(Authentication);

    public string Provider { get; set; } = AuthenticationProvider.None;
}
