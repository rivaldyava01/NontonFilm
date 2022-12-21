using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.CinemaChains.Commands.AddCinemaChain;
using Zeta.NontonFilm.Application.CinemaChains.Commands.DeleteCinemaChain;
using Zeta.NontonFilm.Application.CinemaChains.Commands.UpdateCinemaChain;
using Zeta.NontonFilm.Application.CinemaChains.Queries.GetListCinemaChains;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.CinemaChains.Queries.GetCinemaChain;
using Zeta.NontonFilm.CinemaChains.Queries.GetCinemaChains;
using Zeta.NontonFilm.Shared.CinemaChain.Queries.GetListCinemaChains;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChains;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChain;
using Zeta.NontonFilm.WebApi.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class CinemaChainsController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ItemCreatedResponse>> AddCinemaChain([FromForm] AddCinemaChainCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddCinemaChain), response);
    }

    [HttpPut(ApiEndpoint.V1.CinemaChains.RouteTemplateFor.CinemaChainId)]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult> UpdateCinemaChain([FromRoute] Guid id, [FromForm] UpdateCinemaChainCommand request)
    {
        if (id != request.Id)
        {
            throw new MismatchException(nameof(UpdateCinemaChainCommand.Id), id, request.Id);
        }

        await Mediator.Send(request);

        return NoContent();
    }

    [HttpDelete(ApiEndpoint.V1.CinemaChains.RouteTemplateFor.CinemaChainId)]
    public async Task<ActionResult> DeleteCinemaChain([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteCinemaChainCommand { Id = id });

        return NoContent();
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetCinemaChains_CinemaChain>))]
    public async Task<ActionResult<PaginatedListResponse<GetCinemaChains_CinemaChain>>> GetCinemaChains([FromQuery] GetCinemaChainsQuery request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet(ApiEndpoint.V1.CinemaChains.RouteTemplateFor.CinemaChainId)]
    public async Task<ActionResult<GetCinemaChainResponse>> GetCinemaChain([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetCinemaChainQuery { Id = id });
    }

    [HttpGet(ApiEndpoint.V1.CinemaChains.RouteTemplateFor.CinemaChainList)]
    [AllowAnonymous]
    public async Task<ActionResult<ListResponse<GetListCinemaChains_CinemaChain>>> GetListCinemaChains()
    {
        return await Mediator.Send(new GetListCinemaChainsQuery());
    }
}
