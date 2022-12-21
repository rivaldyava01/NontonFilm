using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.DateAndTime;

namespace Zeta.NontonFilm.Infrastructure.DateAndTime;

public static class DependencyInjection
{
    public static IServiceCollection AddDateAndTimeService(this IServiceCollection services)
    {
        services.AddTransient<IDateAndTimeService, DateAndTimeService>();

        return services;
    }
}
