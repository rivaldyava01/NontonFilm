using Zeta.NontonFilm.Bsui.Services.AppInfo;
using Zeta.NontonFilm.Bsui.Services.Authentication;
using Zeta.NontonFilm.Bsui.Services.Authorization;
using Zeta.NontonFilm.Bsui.Services.External;
using Zeta.NontonFilm.Bsui.Services.FrontEnd;
using Zeta.NontonFilm.Bsui.Services.Geolocation;
using Zeta.NontonFilm.Bsui.Services.Security;
using Zeta.NontonFilm.Bsui.Services.Telemetry;
using Zeta.NontonFilm.Bsui.Services.UI;

namespace Zeta.NontonFilm.Bsui.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddBsui(this IServiceCollection services, IConfiguration configuration)
    {
        #region AppInfo
        services.AddAppInfoService(configuration);
        #endregion AppInfo

        #region Authentication
        services.AddAuthenticationService(configuration);
        #endregion Authentication

        #region Authorization
        services.AddAuthorizationService(configuration);
        #endregion Authorization

        #region External
        services.AddExternalService(configuration);
        #endregion External

        #region Front End
        services.AddFrontEndService(configuration);
        #endregion Front End

        #region Geolocation
        services.AddGeolocationService(configuration);
        #endregion Geolocation

        #region Security
        services.AddSecurityService();
        #endregion Security

        #region Telemetry
        services.AddTelemetryService(configuration);
        #endregion Telemetry

        #region User Interface
        services.AddUIService();
        #endregion User Interface

        return services;
    }

    public static IApplicationBuilder UseBsui(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseFrontEndService(configuration);

        return app;
    }
}
