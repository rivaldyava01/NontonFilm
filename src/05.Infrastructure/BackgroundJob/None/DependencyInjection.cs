using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.BackgroundJob;

namespace Zeta.NontonFilm.Infrastructure.BackgroundJob.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneBackgroundJobService(this IServiceCollection services)
    {
        services.AddTransient<IBackgroundJobService, NoneBackgroundJobService>();

        return services;
    }
}
