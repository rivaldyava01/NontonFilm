﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Storage;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Storage.AzureBlob;

public static class DependencyInjection
{
    public static IServiceCollection AddAzureBlobStorageService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<AzureBlobStorageOptions>(configuration.GetSection(AzureBlobStorageOptions.SectionKey));
        services.AddSingleton<IStorageService, AzureBlobStorageService>();

        var azureBlobStorageOptions = configuration.GetSection(AzureBlobStorageOptions.SectionKey).Get<AzureBlobStorageOptions>();

        healthChecksBuilder.AddAzureBlobStorage(
            connectionString: azureBlobStorageOptions.ConnectionString,
            containerName: azureBlobStorageOptions.ContainerName,
            name: $"{nameof(Storage)} {CommonDisplayTextFor.Service} ({nameof(AzureBlob)})");

        return services;
    }
}
