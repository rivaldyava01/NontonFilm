using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Shows.Commands.AddShow;
using Zeta.NontonFilm.Application.Shows.Commands.DeleteShow;
using Zeta.NontonFilm.Application.Shows.Commands.UpdateShow;
using Zeta.NontonFilm.Application.Shows.Queries.GetShow;
using Zeta.NontonFilm.Shared.Common.Requests;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Shows.Constants;
using Zeta.NontonFilm.Shared.Shows.Queries.GetPastShows;
using Zeta.NontonFilm.Shared.Shows.Queries.GetShow;
using Zeta.NontonFilm.Shared.Shows.Queries.GetUpcomingShows;
using Zeta.NontonFilm.Shows.Queries.GetPastShows;
using Zeta.NontonFilm.Shows.Queries.GetUpcomingShows;
using Zeta.NontonFilm.WebApi.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class ShowsController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ItemCreatedResponse>> AddShow([FromForm] AddShowCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddShow), response);
    }

    [HttpPut(ApiEndpoint.V1.Shows.RouteTemplateFor.ShowId)]
    [AllowAnonymous]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult> UpdateShow([FromRoute] Guid id, [FromForm] UpdateShowCommand request)
    {
        if (id != request.Id)
        {
            throw new MismatchException(nameof(UpdateShowCommand.Id), id, request.Id);
        }

        await Mediator.Send(request);

        return NoContent();
    }

    [HttpDelete(ApiEndpoint.V1.Shows.RouteTemplateFor.ShowId)]
    [AllowAnonymous]
    public async Task<ActionResult> DeleteShow([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteShowCommand { Id = id });

        return NoContent();
    }

    [HttpGet(ApiEndpoint.V1.Shows.RouteTemplateFor.PastShow)]
    [AllowAnonymous]
    [Produces(typeof(PaginatedListResponse<GetPastShows_Show>))]
    public async Task<ActionResult<PaginatedListResponse<GetPastShows_Show>>> GetPastShows([FromQuery] PaginatedListRequest request, [FromRoute] Guid studioId)
    {
        var query = new GetPastShowsQuery
        {
            StudioId = studioId,
            Page = request.Page,
            PageSize = request.PageSize,
            SearchText = request.SearchText,
            SortField = request.SortField,
            SortOrder = request.SortOrder
        };

        return await Mediator.Send(query);
    }

    [HttpGet(ApiEndpoint.V1.Shows.RouteTemplateFor.UpcomingShow)]
    [AllowAnonymous]
    [Produces(typeof(PaginatedListResponse<GetUpcomingShows_Show>))]
    public async Task<ActionResult<PaginatedListResponse<GetUpcomingShows_Show>>> GetUpcominghShows([FromQuery] PaginatedListRequest request, [FromRoute] Guid studioId)
    {
        var query = new GetUpcomingShowsQuery
        {
            StudioId = studioId,
            Page = request.Page,
            PageSize = request.PageSize,
            SearchText = request.SearchText,
            SortField = request.SortField,
            SortOrder = request.SortOrder
        };

        return await Mediator.Send(query);
    }

    [HttpGet(ApiEndpoint.V1.Shows.RouteTemplateFor.ShowId)]
    [AllowAnonymous]
    public async Task<ActionResult<GetShowResponse>> GetSeat([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetShowQuery { Id = id });
    }
}
