using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.BackgroundJob;
using Zeta.NontonFilm.Infrastructure.BackgroundJob.Hangfire.Storages.MySql;
using Zeta.NontonFilm.Infrastructure.BackgroundJob.Hangfire.Storages.SqlServer;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Common.Extensions;

namespace Zeta.NontonFilm.Infrastructure.BackgroundJob.Hangfire;

public static class DependencyInjection
{
    public static IServiceCollection AddHangfireBackgroundJobService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        var hangfireBackgroundJobOptions = configuration.GetSection(HangfireBackgroundJobOptions.SectionKey).Get<HangfireBackgroundJobOptions>();

        switch (hangfireBackgroundJobOptions.Storage.Provider)
        {
            case HangfireBackgroundJobStorageProvider.SqlServer:
                var sqlServerOptions = configuration.GetSection(SqlServerOptions.SectionKey).Get<SqlServerOptions>();
                services.AddHangfireUsingSqlServerDatabase(sqlServerOptions, healthChecksBuilder);
                break;
            case HangfireBackgroundJobStorageProvider.MySql:
                var mySqlOptions = configuration.GetSection(MySqlOptions.SectionKey).Get<MySqlOptions>();
                services.AddHangfireUsingMySqlDatabase(mySqlOptions, healthChecksBuilder);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Hangfire)} {nameof(BackgroundJob).SplitWords()} {nameof(HangfireBackgroundJobOptions.Storage)} {nameof(HangfireBackgroundJobOptions.Storage.Provider)}: {hangfireBackgroundJobOptions.Storage.Provider}");
        }

        services.AddTransient<IBackgroundJobService, HangfireBackgroundJobService>();
        services.AddHangfireServer(options => options.WorkerCount = hangfireBackgroundJobOptions.WorkerCount);

        healthChecksBuilder.AddHangfire(
            setup => setup.MinimumAvailableServers = 1,
            name: $"{nameof(BackgroundJob).SplitWords()} {CommonDisplayTextFor.Service} ({nameof(Hangfire)})");

        return services;
    }

    public static IApplicationBuilder UseHangfireBackgroundJobService(this IApplicationBuilder app, IConfiguration configuration)
    {
        var hangfireBackgroundJobOptions = configuration.GetSection(HangfireBackgroundJobOptions.SectionKey).Get<HangfireBackgroundJobOptions>();

        app.UseHangfireDashboard(hangfireBackgroundJobOptions.Dashboard.Url, new DashboardOptions
        {
            Authorization = new[]
            {
                new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    SslRedirect = false,
                    LoginCaseSensitive = true,
                    Users = new []
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = hangfireBackgroundJobOptions.Dashboard.Username,
                            PasswordClear =  hangfireBackgroundJobOptions.Dashboard.Password
                        }
                    }
                })
            }
        });

        return app;
    }
}
