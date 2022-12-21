using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Shows.Commands.AddShow;
using Zeta.NontonFilm.Shared.Shows.Commands.UpdateShow;
using Zeta.NontonFilm.Shared.Shows.Constants;
using Zeta.NontonFilm.Shared.Shows.Queries.GetPastShows;
using Zeta.NontonFilm.Shared.Shows.Queries.GetShow;
using Zeta.NontonFilm.Shared.Shows.Queries.GetUpcomingShows;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class ShowService
{
    private readonly RestClient _restClient;

    public ShowService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddShowAsync(AddShowRequest request)
    {
        var restRequest = new RestRequest(ApiEndpoint.V1.Shows.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> UpdateShowAsync(UpdateShowRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Shows.Segment}/{request.Id}", Method.Put);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> DeleteShowAsync(Guid studioId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Shows.Segment}/{studioId}", Method.Delete);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetPastShows_Show>>> GetPastShowsAsync(GetPastShowsRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Shows.Segment}/past/{request.StudioId}", Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetPastShows_Show>>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetUpcomingShows_Show>>> GetUpcomingShowsAsync(GetUpcomingShowsRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Shows.Segment}/upcoming/{request.StudioId}", Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetUpcomingShows_Show>>();
    }
    public async Task<ResponseResult<GetShowResponse>> GetShowAsync(Guid id)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Shows.Segment}/{id}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetShowResponse>();
    }
}
