using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.CinemaChain.Queries.GetListCinemaChains;
using Zeta.NontonFilm.Shared.CinemaChains.Commands.AddCinemaChain;
using Zeta.NontonFilm.Shared.CinemaChains.Commands.UpdateCinemaChain;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChain;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChains;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class CinemaChainService
{
    private readonly RestClient _restClient;

    public CinemaChainService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddCinemaChainAsync(AddCinemaChainRequest request)
    {
        var restRequest = new RestRequest(ApiEndpoint.V1.CinemaChains.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> UpdateCinemaChainAsync(UpdateCinemaChainRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.CinemaChains.Segment}/{request.Id}", Method.Put);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> DeleteCinemaChainAsync(Guid customerId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.CinemaChains.Segment}/{customerId}", Method.Delete);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetCinemaChains_CinemaChain>>> GetCinemaChainsAsync(GetCinemaChainsRequest request)
    {
        var restRequest = new RestRequest(ApiEndpoint.V1.CinemaChains.Segment, Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetCinemaChains_CinemaChain>>();
    }

    public async Task<ResponseResult<ListResponse<GetListCinemaChains_CinemaChain>>> GetListCinemaChainsAsync()
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.CinemaChains.Segment}/List", Method.Get);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetListCinemaChains_CinemaChain>>();
    }

    public async Task<ResponseResult<GetCinemaChainResponse>> GetCinemaChainAsync(Guid supplierId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.CinemaChains.Segment}/{supplierId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetCinemaChainResponse>();
    }
}
