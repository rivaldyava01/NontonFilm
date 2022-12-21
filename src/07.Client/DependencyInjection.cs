using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Client.Services.BackEnd;
using Zeta.NontonFilm.Client.Services.HealthCheck;
using Zeta.NontonFilm.Client.Services.UserInfo;

namespace Zeta.NontonFilm.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthCheckService();
        services.AddBackEndService(configuration);
        services.AddUserInfoService();

        return services;
    }
}
