using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Genre.Queries.GetListGenres;
using Zeta.NontonFilm.Shared.Genres.Commands.AddGenre;
using Zeta.NontonFilm.Shared.Genres.Commands.UpdateGenre;
using Zeta.NontonFilm.Shared.Genres.Constants;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;
using static Zeta.NontonFilm.Shared.Genres.Constants.ApiEndpoint.V1;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenres;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class GenreService
{
    private readonly RestClient _restClient;

    public GenreService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddGenreAsync(AddGenreRequest request)
    {
        var restRequest = new RestRequest(ApiEndpoint.V1.Genres.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> UpdateGenreAsync(UpdateGenreRequest request)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Genres.Segment}/{request.Id}", Method.Put);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> DeleteGenreAsync(Guid genreId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Genres.Segment}/{genreId}", Method.Delete);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<GetGenreResponse>> GetGenreAsync(Guid genreId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Genres.Segment}/{genreId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetGenreResponse>();
    }

    public async Task<ResponseResult<ListResponse<GetListGenres_Genre>>> GetListGenresAsync()
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Genres.Segment}/list", Method.Get);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetListGenres_Genre>>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetGenres_Genre>>> GetGenresAsync(GetGenresRequest request)
    {
        var restRequest = new RestRequest(Genres.Segment, Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetGenres_Genre>>();
    }
}
