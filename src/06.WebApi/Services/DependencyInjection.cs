using Zeta.NontonFilm.WebApi.Services.BackEnd;
using Zeta.NontonFilm.WebApi.Services.Documentation;

namespace Zeta.NontonFilm.WebApi.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBackEndService(configuration);
        services.AddDocumentationService(configuration);

        return services;
    }
}
