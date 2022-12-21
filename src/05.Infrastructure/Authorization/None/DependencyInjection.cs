using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Authorization;

namespace Zeta.NontonFilm.Infrastructure.Authorization.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneAuthorizationService(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationService, NoneAuthorizationService>();

        return services;
    }
}
