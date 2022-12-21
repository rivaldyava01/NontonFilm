using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Storage;

namespace Zeta.NontonFilm.Infrastructure.Storage.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneStorageService(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, NoneStorageService>();

        return services;
    }
}
