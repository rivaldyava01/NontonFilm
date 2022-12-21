using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Infrastructure.AppInfo;
using Zeta.NontonFilm.Infrastructure.Authentication;
using Zeta.NontonFilm.Infrastructure.Authorization;
using Zeta.NontonFilm.Infrastructure.BackgroundJob;
using Zeta.NontonFilm.Infrastructure.CurrentUser;
using Zeta.NontonFilm.Infrastructure.DateAndTime;
using Zeta.NontonFilm.Infrastructure.DomainEvent;
using Zeta.NontonFilm.Infrastructure.Email;
using Zeta.NontonFilm.Infrastructure.HealthCheck;
using Zeta.NontonFilm.Infrastructure.Otp;
using Zeta.NontonFilm.Infrastructure.Persistence;
using Zeta.NontonFilm.Infrastructure.QrCode;
using Zeta.NontonFilm.Infrastructure.Sms;
using Zeta.NontonFilm.Infrastructure.Storage;
using Zeta.NontonFilm.Infrastructure.Telemetry;
using Zeta.NontonFilm.Infrastructure.UserProfile;

namespace Zeta.NontonFilm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Health Check
        var healthChecksBuilder = services.AddHealthCheckService(configuration);
        #endregion Health Check

        #region AppInfo
        services.AddAppInfoService(configuration);
        #endregion AppInfo

        #region Authentication
        services.AddAuthenticationService(configuration, healthChecksBuilder);
        #endregion Authentication

        #region Authorization
        services.AddAuthorizationService(configuration, healthChecksBuilder);
        #endregion Authorization

        #region Background Job
        services.AddBackgroundJobService(configuration, healthChecksBuilder);
        #endregion Background Job

        #region Current User
        services.AddCurrentUserService();
        #endregion Current User

        #region DateTime
        services.AddDateAndTimeService();
        #endregion DateTime

        #region Domain Event
        services.AddDomainEventService();
        #endregion Domain Event

        #region Email
        services.AddEmailService(configuration, healthChecksBuilder);
        #endregion Email

        #region One Time Password
        services.AddOtpService();
        #endregion One Time Password

        #region Persistence
        services.AddPersistenceService(configuration, healthChecksBuilder);
        #endregion Persistence

        #region SMS
        services.AddSmsService(configuration, healthChecksBuilder);
        #endregion SMS

        #region Storage
        services.AddStorageService(configuration, healthChecksBuilder);
        #endregion Storage

        #region Telemetry
        services.AddTelemetryService(configuration);
        #endregion Telemetry

        #region User Profile
        services.AddUserProfileService(configuration, healthChecksBuilder);
        #endregion User Profile

        #region QrCode
        services.AddQrCodeService();
        #endregion QrCode

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this WebApplication app, IConfiguration configuration)
    {
        #region Authentication
        app.UseAuthenticationService(configuration);
        #endregion Authentication

        #region Authorization
        app.UseAuthorizationService(configuration);
        #endregion Authorization

        #region Background Job
        app.UseBackgroundJobService(configuration);
        #endregion Background Job

        #region Health Check
        app.UseHealthCheckService(configuration);
        #endregion Health Check

        return app;
    }
}
