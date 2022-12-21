using Zeta.NontonFilm.Shared.Services.Authorization.Models.GetAuthorizationInfo;

namespace Zeta.NontonFilm.Application.Services.Authorization;

public interface IAuthorizationService
{
    Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, CancellationToken cancellationToken);
}
