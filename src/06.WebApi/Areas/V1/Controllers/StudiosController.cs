using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Studios.Commands.AddStudio;
using Zeta.NontonFilm.Studios.Queries.GetStudios;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudios;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.WebApi.Common.Constants;
using Zeta.NontonFilm.Shared.Studios.Constants;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudio;
using Zeta.NontonFilm.Application.Studios.Queries.GetStudio;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Studios.Commands.UpdateStudio;
using Zeta.NontonFilm.Application.Studios.Commands.DeleteStudio;
using Zeta.NontonFilm.Shared.Studio.Queries.GetListStudios;
using Zeta.NontonFilm.Application.Studios.Queries.GetListStudios;
using Zeta.NontonFilm.Shared.Common.Requests;
using Zeta.NontonFilm.Cinemas.Queries.GetCinemas;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class StudiosController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ItemCreatedResponse>> AddStudio([FromForm] AddStudioCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddStudio), response);
    }

    [HttpPut(ApiEndpoint.V1.Studios.RouteTemplateFor.StudioId)]
    [AllowAnonymous]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult> UpdateStudio([FromRoute] Guid id, [FromForm] UpdateStudioCommand request)
    {
        if (id != request.Id)
        {
            throw new MismatchException(nameof(UpdateStudioCommand.Id), id, request.Id);
        }

        await Mediator.Send(request);

        return NoContent();
    }

    [HttpDelete(ApiEndpoint.V1.Studios.RouteTemplateFor.StudioId)]
    [AllowAnonymous]
    public async Task<ActionResult> DeleteStudio([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteStudioCommand { Id = id });

        return NoContent();
    }

    [HttpGet(ApiEndpoint.V1.Studios.RouteTemplateFor.CinemaId)]
    [AllowAnonymous]
    [Produces(typeof(PaginatedListResponse<GetStudios_Studio>))]
    public async Task<ActionResult<PaginatedListResponse<GetStudios_Studio>>> GetStudios([FromQuery] PaginatedListRequest request, [FromRoute] Guid cinemaId)
    {
        var query = new GetStudiosQuery
        {
            CinemaId = cinemaId,
            Page = request.Page,
            PageSize = request.PageSize,
            SearchText = request.SearchText,
            SortField = request.SortField,
            SortOrder = request.SortOrder
        };

        return await Mediator.Send(query);
    }

    [HttpGet("List")]
    [AllowAnonymous]
    public async Task<ActionResult<ListResponse<GetListStudios_Studio>>> GetListStudios()
    {
        return await Mediator.Send(new GetListStudiosQuery());
    }

    [HttpGet(ApiEndpoint.V1.Studios.RouteTemplateFor.StudioId)]
    [AllowAnonymous]
    public async Task<ActionResult<GetStudioResponse>> GetStudio([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetStudioQuery { Id = id });
    }
}
