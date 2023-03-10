using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Services.Authorization;

public class AuthorizationOptions
{
    public const string SectionKey = nameof(Authorization);

    public string Provider { get; set; } = AuthorizationProvider.None;
}
