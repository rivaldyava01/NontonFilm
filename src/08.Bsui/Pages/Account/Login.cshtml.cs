using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Zeta.NontonFilm.Bsui.Common.Pages.Errors.Constants;
using Zeta.NontonFilm.Bsui.Services.Authentication.IS4IM;
using Zeta.NontonFilm.Bsui.Services.Authentication.ZetaGarde;
using Zeta.NontonFilm.Bsui.Services.Authorization;
using Zeta.NontonFilm.Bsui.Services.Authorization.IS4IM;
using Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde;
using Zeta.NontonFilm.Client.Services.HealthCheck;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Services.HealthCheck.Constants;
using AuthenticationOptions = Zeta.NontonFilm.Bsui.Services.Authentication.AuthenticationOptions;

namespace Zeta.NontonFilm.Bsui.Pages.Account;

public class LoginModel : PageModel
{
    private readonly HealthCheckService _healthCheckService;
    private readonly bool _usingAuthentication;
    private readonly string? _authenticationHealthCheckUrl;
    private readonly string? _authorizationHealthCheckUrl;

    public LoginModel(
        HealthCheckService healthCheckService,
        IOptions<AuthenticationOptions> authenticationOptions,
        IOptions<AuthorizationOptions> authorizationOptions,
        IConfiguration configuration)
    {
        _healthCheckService = healthCheckService;
        _usingAuthentication = authenticationOptions.Value.Provider is not AuthenticationProvider.None;

        switch (authenticationOptions.Value.Provider)
        {
            case AuthenticationProvider.None:
                _authenticationHealthCheckUrl = null;
                break;
            case AuthenticationProvider.ZetaGarde:
                var zetaGardeAuthenticationOptions = configuration.GetSection(ZetaGardeAuthenticationOptions.SectionKey).Get<ZetaGardeAuthenticationOptions>();
                _authenticationHealthCheckUrl = zetaGardeAuthenticationOptions.HealthCheckUrl;
                break;
            case AuthenticationProvider.IS4IM:
                var is4imAuthenticationOptions = configuration.GetSection(IS4IMAuthenticationOptions.SectionKey).Get<IS4IMAuthenticationOptions>();
                _authenticationHealthCheckUrl = is4imAuthenticationOptions.HealthCheckUrl;
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {AuthenticationDisplayTextFor.AuthenticationProvider}: {authenticationOptions.Value.Provider}");
        }

        switch (authorizationOptions.Value.Provider)
        {
            case AuthorizationProvider.None:
                _authenticationHealthCheckUrl = null;
                break;
            case AuthorizationProvider.ZetaGarde:
                var zetaGardeAuthorizationOptions = configuration.GetSection(ZetaGardeAuthorizationOptions.SectionKey).Get<ZetaGardeAuthorizationOptions>();
                _authorizationHealthCheckUrl = zetaGardeAuthorizationOptions.HealthCheckUrl;
                break;
            case AuthorizationProvider.IS4IM:
                var is4imAuthorizationOptions = configuration.GetSection(IS4IMAuthorizationOptions.SectionKey).Get<IS4IMAuthorizationOptions>();
                _authorizationHealthCheckUrl = is4imAuthorizationOptions.HealthCheckUrl;
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {AuthorizationDisplayTextFor.AuthorizationProvider}: {authorizationOptions.Value.Provider}");
        }
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl)
    {
        if (!_usingAuthentication)
        {
            Response.Redirect(returnUrl);
        }

        if (!string.IsNullOrWhiteSpace(_authenticationHealthCheckUrl))
        {
            var authenticationHealthCheckResponse = await _healthCheckService.GetHealthCheckAsync(_authenticationHealthCheckUrl);

            if (authenticationHealthCheckResponse.Status is not HealthCheckStatus.Healthy)
            {
                return Redirect($"~/{RouteFor.CannotReachAuthenticationProvider}");
            }
        }

        if (!string.IsNullOrWhiteSpace(_authorizationHealthCheckUrl))
        {
            var authorizationHealthCheckResponse = await _healthCheckService.GetHealthCheckAsync(_authorizationHealthCheckUrl);

            if (authorizationHealthCheckResponse.Status is not HealthCheckStatus.Healthy)
            {
                return Redirect($"~/{RouteFor.CannotReachAuthorizationProvider}");
            }
        }

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            returnUrl = Url.Content("~/");
        }

        if (HttpContext.User.Identity is not null)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(returnUrl);
            }
        }

        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, OpenIdConnectDefaults.AuthenticationScheme);
    }
}
