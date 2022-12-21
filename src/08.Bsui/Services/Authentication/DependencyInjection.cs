﻿using Microsoft.AspNetCore.Components.Authorization;
using Zeta.NontonFilm.Bsui.Services.Authentication.ZetaGarde;
using Zeta.NontonFilm.Bsui.Services.Authentication.IS4IM;
using Zeta.NontonFilm.Bsui.Services.Authentication.None;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;

namespace Zeta.NontonFilm.Bsui.Services.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionKey));
        services.AddScoped<AuthenticationStateProvider, AuthorizedAuthenticationStateProvider>();

        var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionKey).Get<AuthenticationOptions>();

        switch (authenticationOptions.Provider)
        {
            case AuthenticationProvider.None:
                services.AddNoneAuthenticationService();
                break;
            case AuthenticationProvider.ZetaGarde:
                services.AddZetaGardeAuthentication(configuration);
                break;
            case AuthenticationProvider.IS4IM:
                services.AddIS4IMAuthentication(configuration);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Authentication)} {nameof(AuthenticationOptions.Provider)}: {authenticationOptions.Provider}");
        }

        return services;
    }

    public static IApplicationBuilder UseAuthenticationService(this IApplicationBuilder app, IConfiguration configuration)
    {
        var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionKey).Get<AuthenticationOptions>();

        switch (authenticationOptions.Provider)
        {
            case AuthenticationProvider.None:
                break;
            case AuthenticationProvider.ZetaGarde:
                app.UseZetaGardeAuthentication(configuration);
                break;
            case AuthenticationProvider.IS4IM:
                app.UseIS4IMAuthentication();
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Authentication)} {nameof(AuthenticationOptions.Provider)}: {authenticationOptions.Provider}");
        }

        return app;
    }
}
