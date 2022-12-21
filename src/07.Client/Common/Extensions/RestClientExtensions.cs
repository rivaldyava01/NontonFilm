using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Common.Constants;
using RestSharp;

namespace Zeta.NontonFilm.Client.Common.Extensions;

public static class RestClientExtensions
{
    private const string Bearer = nameof(Bearer);

    public static void AddUserInfo(this RestClient restClient, UserInfoService userInfoService)
    {
        restClient.AddDefaultHeader(HttpHeaderName.Authorization, $"{Bearer} {userInfoService.AccessToken}");

        if (!string.IsNullOrWhiteSpace(userInfoService.PositionId))
        {
            restClient.AddDefaultHeader(HttpHeaderName.ZtcbPositionId, userInfoService.PositionId);
        }

        if (!string.IsNullOrWhiteSpace(userInfoService.IpAddress))
        {
            restClient.AddDefaultHeader(HttpHeaderName.ZtcbIpAddress, userInfoService.IpAddress);
        }

        if (userInfoService.Geolocation is not null)
        {
            restClient.AddDefaultHeader(HttpHeaderName.ZtcbGeolocation, userInfoService.Geolocation.ToString());
        }
    }
}
