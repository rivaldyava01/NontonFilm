using Zeta.NontonFilm.Bsui.Services.External.Location;

namespace Zeta.NontonFilm.Bsui.Services.External;

public static class DependencyInjection
{
    public static IServiceCollection AddExternalService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLocationExternalService(configuration);

        return services;
    }
}
