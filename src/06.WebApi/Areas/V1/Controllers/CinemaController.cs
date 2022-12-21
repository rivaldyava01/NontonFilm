using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Cinemas.Commands.AddCinema;
using Zeta.NontonFilm.Application.Cinemas.Commands.DeleteCinema;
using Zeta.NontonFilm.Application.Cinemas.Commands.UpdateCinema;
using Zeta.NontonFilm.Application.Cinemas.Queries.GetCinema;
using Zeta.NontonFilm.Application.Cinemas.Queries.GetCinemasForUser;
using Zeta.NontonFilm.Application.Cinemas.Queries.GetCinemasForUserByMovieId;
using Zeta.NontonFilm.Application.Cinemas.Queries.GetListCinemas;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Cinemas.Queries.GetCinemas;
using Zeta.NontonFilm.Shared.Cinema.Queries.GetListCinemas;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinema;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemas;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUser;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUserByMovieId;
using Zeta.NontonFilm.Shared.Common.Requests;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.WebApi.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class CinemasController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ItemCreatedResponse>> AddCinema([FromForm] AddCinemaCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddCinema), response);
    }

    [HttpPut(ApiEndpoint.V1.Cinemas.RouteTemplateFor.CinemaId)]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult> UpdateCinema([FromRoute] Guid id, [FromForm] UpdateCinemaCommand request)
    {
        if (id != request.Id)
        {
            throw new MismatchException(nameof(UpdateCinemaCommand.Id), id, request.Id);
        }

        await Mediator.Send(request);

        return NoContent();
    }

    [HttpDelete(ApiEndpoint.V1.Cinemas.RouteTemplateFor.CinemaId)]
    public async Task<ActionResult> DeleteCinema([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteCinemaCommand { Id = id });

        return NoContent();
    }

    [HttpGet(ApiEndpoint.V1.Cinemas.RouteTemplateFor.CinemaChainId)]
    [Produces(typeof(PaginatedListResponse<GetCinemas_Cinema>))]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult<PaginatedListResponse<GetCinemas_Cinema>>> GetCinemas([FromQuery] PaginatedListRequest request, [FromRoute] Guid cinemaChainId)
    {
        var query = new GetCinemasQuery
        {
            CinemaChainId = cinemaChainId,
            Page = request.Page,
            PageSize = request.PageSize,
            SearchText = request.SearchText,
            SortField = request.SortField,
            SortOrder = request.SortOrder
        };

        return await Mediator.Send(query);
    }

    [HttpGet(ApiEndpoint.V1.Cinemas.RouteTemplateFor.CinemaId)]
    public async Task<ActionResult<GetCinemaResponse>> GetCinema([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetCinemaQuery { Id = id });
    }

    [HttpGet(ApiEndpoint.V1.Cinemas.RouteTemplateFor.CinemaList)]
    [AllowAnonymous]
    public async Task<ActionResult<ListResponse<GetListCinemas_Cinema>>> GetListCinemas()
    {
        return await Mediator.Send(new GetListCinemasQuery());
    }

    [HttpGet(ApiEndpoint.V1.Cinemas.RouteTemplateFor.CityId)]
    public async Task<ActionResult<ListResponse<GetCinemasForUser_Cinema>>> GetCinemasForUser([FromRoute] Guid cityId)
    {
        return await Mediator.Send(new GetCinemasForUserQuery { CityId = cityId });
    }

    [HttpGet(ApiEndpoint.V1.Cinemas.RouteTemplateFor.MovieId)]
    [Produces(typeof(ListResponse<GetCinemasForUserByMovieId_Cinema>))]
    public async Task<ActionResult<ListResponse<GetCinemasForUserByMovieId_Cinema>>> GetCinemasForUserByMovieId([FromQuery] GetCinemasForUserByMovieIdRequest request, [FromRoute] Guid movieId)
    {
        var query = new GetCinemasForUserByMovieIdQuery
        {
            CityId = request.CityId,
            MovieId = request.MovieId
        };

        return await Mediator.Send(query);
    }
}
