using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Seats.Queries.GetSeat;
using Zeta.NontonFilm.Shared.Seats.Queries.GetSeats;
using static Zeta.NontonFilm.Shared.Seats.Constants.ApiEndpoint.V1;

namespace Zeta.NontonFilm.Client.Services.BackEnd;
public class SeatService
{
    private readonly RestClient _restClient;

    public SeatService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<GetSeatsResponse>> GetSeatsAsync(Guid id)
    {
        var restRequest = new RestRequest($"{Seats.Segment}/total/{id}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);
        return restResponse.ToResponseResult<GetSeatsResponse>();
    }
}
