﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Zeta.NontonFilm.Infrastructure.Logging.None;
using Zeta.NontonFilm.Infrastructure.Logging.Serilog;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Logging;

public static class DependencyInjection
{
    public static IHostBuilder UseLoggingService(this IHostBuilder hostBuilder)
    {
        var configuration = new ConfigurationBuilder()
          .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{CommonValueFor.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables()
          .Build();

        var loggingOptions = configuration.GetSection(LoggingOptions.SectionKey).Get<LoggingOptions>();

        switch (loggingOptions.Provider)
        {
            case LoggingProvider.None:
                hostBuilder.UseNoneLoggingService();
                break;
            case LoggingProvider.Serilog:
                hostBuilder.UseSerilogLoggingService();
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Logging)} {nameof(LoggingOptions.Provider)}: {loggingOptions.Provider}");
        }

        return hostBuilder;
    }
}
