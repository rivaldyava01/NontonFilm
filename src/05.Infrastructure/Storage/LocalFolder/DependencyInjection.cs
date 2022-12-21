using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Application.Services.Storage;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Storage.LocalFolder;

public static class DependencyInjection
{
    public static IServiceCollection AddLocalFolderStorageService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<LocalFolderStorageOptions>(configuration.GetSection(LocalFolderStorageOptions.SectionKey));
        services.AddSingleton<IStorageService, LocalFolderStorageService>();

        var localFolderStorageOptions = configuration.GetSection(LocalFolderStorageOptions.SectionKey).Get<LocalFolderStorageOptions>();

        if (!Directory.Exists(localFolderStorageOptions.FolderPath))
        {
            Directory.CreateDirectory(localFolderStorageOptions.FolderPath);
        }

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Storage)} {CommonDisplayTextFor.Service} ({nameof(LocalFolder)})",
            instance: new LocalFolderStorageHealthCheck(localFolderStorageOptions.FolderPath),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
