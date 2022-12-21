using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Authorization;
using Zeta.NontonFilm.Infrastructure.Authorization.ZetaGarde;
using Zeta.NontonFilm.Infrastructure.Authorization.IS4IM;
using Zeta.NontonFilm.Infrastructure.Authorization.None;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Infrastructure.Authorization;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<AuthorizationOptions>(configuration.GetSection(AuthorizationOptions.SectionKey));

        var authorizationOptions = configuration.GetSection(AuthorizationOptions.SectionKey).Get<AuthorizationOptions>();

        switch (authorizationOptions.Provider)
        {
            case AuthorizationProvider.None:
                services.AddNoneAuthorizationService();
                break;
            case AuthorizationProvider.ZetaGarde:
                services.AddZetaGardeAuthorizationService(configuration, healthChecksBuilder);
                break;
            case AuthorizationProvider.IS4IM:
                services.AddIS4IMAuthorizationService(configuration, healthChecksBuilder);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Authorization)} {nameof(AuthorizationOptions.Provider)}: {authorizationOptions.Provider}");
        }

        return services;
    }

    public static IApplicationBuilder UseAuthorizationService(this WebApplication app, IConfiguration configuration)
    {
        var authorizationOptions = configuration.GetSection(AuthorizationOptions.SectionKey).Get<AuthorizationOptions>();

        switch (authorizationOptions.Provider)
        {
            case AuthorizationProvider.None:
                app.MapControllers();
                break;
            case AuthorizationProvider.ZetaGarde:
                app.UseAuthorization();
                app.MapControllers().RequireAuthorization();
                break;
            case AuthorizationProvider.IS4IM:
                app.UseAuthorization();
                app.MapControllers().RequireAuthorization();
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Authorization)} {nameof(AuthorizationOptions.Provider)}: {authorizationOptions.Provider}");
        }

        return app;
    }
}
