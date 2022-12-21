using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public static class DependencyInjection
{
    public static IServiceCollection AddBackEndService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BackEndOptions>(configuration.GetSection(BackEndOptions.SectionKey));

        #region Essential Services
        services.AddTransient<AuditService>();
        #endregion Essential Services

        #region Business Services
        services.AddTransient<CinemaService>();
        services.AddTransient<CinemaChainService>();
        services.AddTransient<CityService>();
        services.AddTransient<GenreService>();
        services.AddTransient<MovieService>();
        services.AddTransient<ShowService>();
        services.AddTransient<StudioService>();
        services.AddTransient<SeatService>();
        services.AddTransient<TicketService>();
        #endregion Business Services

        return services;
    }
}
