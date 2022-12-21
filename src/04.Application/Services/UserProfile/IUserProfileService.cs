using Zeta.NontonFilm.Application.Services.UserProfile.Models.GetUserProfile;

namespace Zeta.NontonFilm.Application.Services.UserProfile;

public interface IUserProfileService
{
    Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken);
}
