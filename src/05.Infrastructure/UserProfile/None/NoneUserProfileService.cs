using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Application.Services.UserProfile;
using Zeta.NontonFilm.Application.Services.UserProfile.Models.GetUserProfile;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Common.Extensions;

namespace Zeta.NontonFilm.Infrastructure.UserProfile.None;

public class NoneUserProfileService : IUserProfileService
{
    private readonly ILogger<NoneUserProfileService> _logger;

    public NoneUserProfileService(ILogger<NoneUserProfileService> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(UserProfile).SplitWords()} {CommonDisplayTextFor.Service}");
    }

    public Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken)
    {
        LogWarning();

        return Task.FromResult(new GetUserProfileResponse());
    }
}
