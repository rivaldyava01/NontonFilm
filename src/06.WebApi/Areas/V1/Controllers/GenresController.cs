using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Genres.Commands.AddGenre;
using Zeta.NontonFilm.Application.Genres.Commands.DeleteGenre;
using Zeta.NontonFilm.Application.Genres.Commands.UpdateGenre;
using Zeta.NontonFilm.Application.Genres.Queries.GetGenre;
using Zeta.NontonFilm.Application.Genres.Queries.GetListGenres;
using Zeta.NontonFilm.Genres.Queries.GetGenres;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Genre.Queries.GetListGenres;
using Zeta.NontonFilm.Shared.Genres.Constants;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenres;
using Zeta.NontonFilm.WebApi.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class GenresController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ItemCreatedResponse>> AddGenre([FromForm] AddGenreCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddGenre), response);
    }

    [HttpPut(ApiEndpoint.V1.Genres.RouteTemplateFor.GenreId)]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult> UpdateGenre([FromRoute] Guid id, [FromForm] UpdateGenreCommand request)
    {
        if (id != request.Id)
        {
            throw new MismatchException(nameof(UpdateGenreCommand.Id), id, request.Id);
        }

        await Mediator.Send(request);

        return NoContent();
    }

    [HttpDelete(ApiEndpoint.V1.Genres.RouteTemplateFor.GenreId)]
    public async Task<ActionResult> DeleteGenre([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteGenreCommand { Id = id });

        return NoContent();
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetGenres_Genre>))]
    public async Task<ActionResult<PaginatedListResponse<GetGenres_Genre>>> GetGenres([FromQuery] GetGenresQuery request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet("List")]
    [AllowAnonymous]
    public async Task<ActionResult<ListResponse<GetListGenres_Genre>>> GetListGenres()
    {
        return await Mediator.Send(new GetListGenresQuery());
    }

    [HttpGet(ApiEndpoint.V1.Genres.RouteTemplateFor.GenreId)]
    public async Task<ActionResult<GetGenreResponse>> GetGenre([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetGenreQuery { Id = id });
    }
}
