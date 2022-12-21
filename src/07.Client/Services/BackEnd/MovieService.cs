using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Movie.Queries.GetListMovies;
using Zeta.NontonFilm.Shared.Movies.Commands.AddMovie;
using Zeta.NontonFilm.Shared.Movies.Commands.UpdateMovie;
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovie;
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovies;
using Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovie;
using Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovies;
using static Zeta.NontonFilm.Shared.Movies.Constants.ApiEndpoint.V1;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class MovieService
{
    private readonly RestClient _restClient;

    public MovieService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddMovieAsync(AddMovieRequest request)
    {
        var restRequest = new RestRequest(Movies.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }
    public async Task<ResponseResult<NoContentResponse>> UpdateMovieAsync(UpdateMovieRequest request)
    {
        var restRequest = new RestRequest($"{Movies.Segment}/{request.Id}", Method.Put);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<NoContentResponse>> DeleteMovieAsync(Guid movieId)
    {
        var restRequest = new RestRequest($"{Movies.Segment}/{movieId}", Method.Delete);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<NoContentResponse>();
    }

    public async Task<ResponseResult<ListResponse<GetListMovies_Movie>>> GetListMoviesAsync()
    {
        var restRequest = new RestRequest($"{Movies.Segment}/List", Method.Get);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetListMovies_Movie>>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetMovies_Movie>>> GetMoviesAsync(GetMoviesRequest request)
    {
        var restRequest = new RestRequest(Movies.Segment, Method.Get);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetMovies_Movie>>();
    }

    public async Task<ResponseResult<GetMovieResponse>> GetMovieAsync(Guid movieId)
    {
        var restRequest = new RestRequest($"{Movies.Segment}/{movieId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetMovieResponse>();
    }

    public async Task<ResponseResult<ListResponse<GetNowShowingMovies_Movie>>> GetNowShowingMoviesAsync()
    {
        var restRequest = new RestRequest($"{Movies.Segment}/nowshowing", Method.Get);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetNowShowingMovies_Movie>>();
    }

    public async Task<ResponseResult<GetNowShowingMovieResponse>> GetNowShowingMovieAsync(Guid movieId)
    {
        var restRequest = new RestRequest($"{Movies.Segment}/nowshowing/{movieId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetNowShowingMovieResponse>();
    }
}
