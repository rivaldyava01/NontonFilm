using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Cinema.Queries.GetListCinemas;
using Zeta.NontonFilm.Shared.Cinemas.Commands.AddCinema;
using Zeta.NontonFilm.Shared.Cinemas.Commands.UpdateCinema;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinema;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemas;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUser;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUserByMovieId;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class CinemaService
{
    private readonly RestClient _restClient;

    public CinemaService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddCinemaAsync(AddCinemaRequest request)
    {
        var restRequest = new RestRequest(ApiEndpoint.V1.Cinemas.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> UpdateCinemaAsync(UpdateCinemaRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Cinemas.Segment}/{request.Id}", Method.Put);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> DeleteCinemaAsync(Guid customerId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Cinemas.Segment}/{customerId}", Method.Delete);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetCinemas_Cinema>>> GetCinemasAsync(GetCinemasRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Cinemas.Segment}/cinemachain/{request.CinemaChainId}", Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetCinemas_Cinema>>();
    }

    public async Task<ResponseResult<ListResponse<GetListCinemas_Cinema>>> GetListCinemasAsync()
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Cinemas.Segment}/List", Method.Get);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetListCinemas_Cinema>>();
    }

    public async Task<ResponseResult<GetCinemaResponse>> GetCinemaAsync(Guid cinemaId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Cinemas.Segment}/{cinemaId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetCinemaResponse>();
    }

    public async Task<ResponseResult<ListResponse<GetCinemasForUser_Cinema>>> GetCinemaForUserAsync(Guid cityId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Cinemas.Segment}/city/{cityId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetCinemasForUser_Cinema>>();
    }

    public async Task<ResponseResult<ListResponse<GetCinemasForUserByMovieId_Cinema>>> GetCinemaForUserByMovieIdAsync(GetCinemasForUserByMovieIdRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Cinemas.Segment}/movie/{request.MovieId}", Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetCinemasForUserByMovieId_Cinema>>();
    }
}
