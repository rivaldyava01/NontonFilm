using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.UserProfile;

namespace Zeta.NontonFilm.Infrastructure.UserProfile.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneUserProfileService(this IServiceCollection services)
    {
        services.AddTransient<IUserProfileService, NoneUserProfileService>();

        return services;
    }
}
