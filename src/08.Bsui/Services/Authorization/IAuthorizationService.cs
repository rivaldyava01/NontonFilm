﻿using Zeta.NontonFilm.Shared.Services.Authorization.Models.GetAuthorizationInfo;
using Zeta.NontonFilm.Shared.Services.Authorization.Models.GetPositions;

namespace Zeta.NontonFilm.Bsui.Services.Authorization;

public interface IAuthorizationService
{
    Task<GetPositionsResponse> GetPositionsAsync(string username, string accessToken, CancellationToken cancellationToken = default);
    Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, string accessToken, CancellationToken cancellationToken = default);
}
