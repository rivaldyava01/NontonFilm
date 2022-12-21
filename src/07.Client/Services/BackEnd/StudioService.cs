using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Studios.Commands.AddStudio;
using Zeta.NontonFilm.Shared.Studios.Commands.UpdateStudio;
using Zeta.NontonFilm.Shared.Studios.Constants;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudio;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudios;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class StudioService
{
    private readonly RestClient _restClient;

    public StudioService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddStudioAsync(AddStudioRequest request)
    {
        var restRequest = new RestRequest(ApiEndpoint.V1.Studios.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> UpdateStudioAsync(UpdateStudioRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Studios.Segment}/{request.Id}", Method.Put);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> DeleteStudioAsync(Guid studioId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Studios.Segment}/{studioId}", Method.Delete);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetStudios_Studio>>> GetStudiosAsync(GetStudiosRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Studios.Segment}/cinema/{request.CinemaId}", Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetStudios_Studio>>();
    }

    public async Task<ResponseResult<GetStudioResponse>> GetStudioAsync(Guid studioId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Studios.Segment}/{studioId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetStudioResponse>();
    }
}
