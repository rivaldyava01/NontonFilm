using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Persistence;

namespace Zeta.NontonFilm.Infrastructure.Persistence.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNonePersistenceService(this IServiceCollection services)
    {
        services.AddScoped<INontonFilmDbContext, NoneNontonFilmDbContext>();

        return services;
    }
}
