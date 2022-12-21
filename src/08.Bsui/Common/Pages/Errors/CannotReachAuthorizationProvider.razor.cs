using Zeta.NontonFilm.Bsui.Services.Authorization.IS4IM;
using Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Bsui.Common.Pages.Errors;

public partial class CannotReachAuthorizationProvider
{
    private string _authorizationProviderUrl = default!;

    protected override void OnInitialized()
    {
        switch (_authorizationOptions.Value.Provider)
        {
            case AuthorizationProvider.None:
                break;
            case AuthorizationProvider.IS4IM:
                var is4imAuthorizationOptions = configuration.GetSection(IS4IMAuthorizationOptions.SectionKey).Get<IS4IMAuthorizationOptions>();
                _authorizationProviderUrl = is4imAuthorizationOptions.BaseUrl;
                break;
            case AuthorizationProvider.ZetaGarde:
                var zetaGardeAuthorizationOptions = configuration.GetSection(ZetaGardeAuthorizationOptions.SectionKey).Get<ZetaGardeAuthorizationOptions>();
                _authorizationProviderUrl = zetaGardeAuthorizationOptions.BaseUrl;
                break;
            default:
                break;
        }
    }
}
