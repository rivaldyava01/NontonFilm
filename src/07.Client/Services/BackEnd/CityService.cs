using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Cities.Commands.AddCity;
using Zeta.NontonFilm.Shared.Cities.Commands.UpdateCity;
using Zeta.NontonFilm.Shared.Cities.Queries.GetCities;
using Zeta.NontonFilm.Shared.Cities.Queries.GetMovies;
using Zeta.NontonFilm.Shared.City.Queries.GetListCities;
using Zeta.NontonFilm.Shared.Common.Responses;
using static Zeta.NontonFilm.Shared.Cities.Constants.ApiEndpoint.V1;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class CityService
{
    private readonly RestClient _restClient;

    public CityService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddCityAsync(AddCityRequest request)
    {
        var restRequest = new RestRequest(Cities.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> UpdateCityAsync(UpdateCityRequest request)
    {
        var restRequest = new RestRequest($"{Cities.Segment}/{request.Id}", Method.Put);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> DeleteCityAsync(Guid cityId)
    {
        var restRequest = new RestRequest($"{Cities.Segment}/{cityId}", Method.Delete);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<ListResponse<GetListCities_City>>> GetListCitiesAsync()
    {
        var restRequest = new RestRequest($"{Cities.Segment}/list", Method.Get);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetListCities_City>>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetCities_City>>> GetCitiesAsync(GetCitiesRequest request)
    {
        var restRequest = new RestRequest(Cities.Segment, Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetCities_City>>();
    }

    public async Task<ResponseResult<ListResponse<GetCities_CityForUser>>> GetCitiesForUserAsync()
    {
        var restRequest = new RestRequest($"{Cities.Segment}/user/list", Method.Get);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetCities_CityForUser>>();
    }
}
