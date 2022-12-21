using Zeta.NontonFilm.Bsui.Services.Authentication.IS4IM;
using Zeta.NontonFilm.Bsui.Services.Authentication.ZetaGarde;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;

namespace Zeta.NontonFilm.Bsui.Common.Pages.Errors;

public partial class CannotReachAuthenticationProvider
{
    private string _authenticationProviderUrl = default!;

    protected override void OnInitialized()
    {
        switch (_authenticationOptions.Value.Provider)
        {
            case AuthenticationProvider.None:
                break;
            case AuthenticationProvider.IS4IM:
                var is4imAuthenticationOptions = configuration.GetSection(IS4IMAuthenticationOptions.SectionKey).Get<IS4IMAuthenticationOptions>();
                _authenticationProviderUrl = is4imAuthenticationOptions.AuthorityUrl;
                break;
            case AuthenticationProvider.ZetaGarde:
                var zetaGardeAuthenticationOptions = configuration.GetSection(ZetaGardeAuthenticationOptions.SectionKey).Get<ZetaGardeAuthenticationOptions>();
                _authenticationProviderUrl = zetaGardeAuthenticationOptions.AuthorityUrl;
                break;
            default:
                break;
        }
    }
}
