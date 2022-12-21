using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.CurrentUser;

namespace Zeta.NontonFilm.Infrastructure.CurrentUser;

public static class DependencyInjection
{
    public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
