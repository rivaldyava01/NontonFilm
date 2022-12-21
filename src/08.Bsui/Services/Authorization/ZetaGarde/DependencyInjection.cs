namespace Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde;

public static class DependencyInjection
{
    public static IServiceCollection AddZetaGardeAuthorizationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ZetaGardeAuthorizationOptions>(configuration.GetSection(ZetaGardeAuthorizationOptions.SectionKey));
        services.AddTransient<IAuthorizationService, ZetaGardeAuthorizationService>();

        return services;
    }
}
