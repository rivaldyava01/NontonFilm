using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Infrastructure.UserProfile.ZetaGarde;
using Zeta.NontonFilm.Infrastructure.UserProfile.IS4IM;
using Zeta.NontonFilm.Infrastructure.UserProfile.None;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Common.Extensions;

namespace Zeta.NontonFilm.Infrastructure.UserProfile;

public static class DependencyInjection
{
    public static IServiceCollection AddUserProfileService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        var userProfileOptions = configuration.GetSection(UserProfileOptions.SectionKey).Get<UserProfileOptions>();

        switch (userProfileOptions.Provider)
        {
            case UserProfileProvider.None:
                services.AddNoneUserProfileService();
                break;
            case UserProfileProvider.ZetaGarde:
                services.AddZetaGardeUserProfileService(configuration, healthChecksBuilder);
                break;
            case UserProfileProvider.IS4IM:
                services.AddIS4IMUserProfileService(configuration, healthChecksBuilder);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(UserProfile).SplitWords()} {nameof(UserProfileOptions.Provider)}: {userProfileOptions.Provider}");
        }

        return services;
    }
}
