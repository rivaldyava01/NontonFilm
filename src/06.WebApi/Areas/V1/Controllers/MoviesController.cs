using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Movies.Commands.AddMovie;
using Zeta.NontonFilm.Application.Movies.Commands.DeleteMovie;
using Zeta.NontonFilm.Application.Movies.Commands.UpdateMovie;
using Zeta.NontonFilm.Application.Movies.Queries.GetListMovies;
using Zeta.NontonFilm.Application.Movies.Queries.GetMovie;
using Zeta.NontonFilm.Application.Movies.Queries.GetNowShowingMovies;
using Zeta.NontonFilm.Movies.Queries.GetMovies;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Movie.Queries.GetListMovies;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovie;
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovies;
using Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovie;
using Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovies;
using Zeta.NontonFilm.WebApi.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class MoviesController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ItemCreatedResponse>> AddMovie([FromForm] AddMovieCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddMovie), response);
    }

    [HttpPut(ApiEndpoint.V1.Movies.RouteTemplateFor.MovieId)]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult> UpdateMovie([FromRoute] Guid id, [FromForm] UpdateMovieCommand request)
    {
        if (id != request.Id)
        {
            throw new MismatchException(nameof(UpdateMovieCommand.Id), id, request.Id);
        }

        await Mediator.Send(request);

        return NoContent();
    }

    [HttpDelete(ApiEndpoint.V1.Movies.RouteTemplateFor.MovieId)]
    public async Task<ActionResult> DeleteMovie([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteMovieCommand { Id = id });

        return NoContent();
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetMovies_Movie>))]
    public async Task<ActionResult<PaginatedListResponse<GetMovies_Movie>>> GetMovies([FromQuery] GetMoviesQuery request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet(ApiEndpoint.V1.Movies.RouteTemplateFor.MovieId)]
    [AllowAnonymous]
    public async Task<ActionResult<GetMovieResponse>> GetMovie([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetMovieQuery { Id = id });
    }

    [HttpGet(ApiEndpoint.V1.Movies.RouteTemplateFor.List)]
    [AllowAnonymous]
    public async Task<ActionResult<ListResponse<GetListMovies_Movie>>> GetListMovies()
    {
        return await Mediator.Send(new GetListMoviesQuery());
    }

    [HttpGet(ApiEndpoint.V1.Movies.RouteTemplateFor.NowShowing)]
    public async Task<ActionResult<ListResponse<GetNowShowingMovies_Movie>>> GetNowShowingMovies()
    {
        return await Mediator.Send(new GetNowShowingMoviesQuery());
    }

    [HttpGet(ApiEndpoint.V1.Movies.RouteTemplateFor.MovieNowShowing)]
    [AllowAnonymous]
    public async Task<ActionResult<GetNowShowingMovieResponse>> GetNowShowingMovie([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetNowShowingMovieQuery { Id = id });
    }
}
