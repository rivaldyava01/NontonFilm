using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Cities.Commands.AddCity;
using Zeta.NontonFilm.Application.Cities.Commands.DeleteCity;
using Zeta.NontonFilm.Application.Cities.Commands.UpdateCity;
using Zeta.NontonFilm.Application.Cities.Queries.GetCity;
using Zeta.NontonFilm.Application.Cities.Queries.GetListCities;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Cities.Queries.GetCities;
using Zeta.NontonFilm.Shared.Cities.Constants;
using Zeta.NontonFilm.Shared.Cities.Queries.GetCity;
using Zeta.NontonFilm.Shared.Cities.Queries.GetMovies;
using Zeta.NontonFilm.Shared.City.Queries.GetListCities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.WebApi.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class CitiesController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ItemCreatedResponse>> AddCity([FromForm] AddCityCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddCity), response);
    }

    [HttpPut(ApiEndpoint.V1.Cities.RouteTemplateFor.CityId)]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult> UpdateCity([FromRoute] Guid id, [FromForm] UpdateCityCommand request)
    {
        if (id != request.Id)
        {
            throw new MismatchException(nameof(UpdateCityCommand.Id), id, request.Id);
        }

        await Mediator.Send(request);

        return NoContent();
    }

    [HttpDelete(ApiEndpoint.V1.Cities.RouteTemplateFor.CityId)]
    public async Task<ActionResult> DeleteCity([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteCityCommand { Id = id });

        return NoContent();
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetCities_City>))]
    public async Task<ActionResult<PaginatedListResponse<GetCities_City>>> GetCities([FromQuery] GetCitiesQuery request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet(ApiEndpoint.V1.Cities.RouteTemplateFor.CityList)]
    [AllowAnonymous]
    public async Task<ActionResult<ListResponse<GetListCities_City>>> GetListCities()
    {
        return await Mediator.Send(new GetListCitiesQuery());
    }

    [HttpGet(ApiEndpoint.V1.Cities.RouteTemplateFor.CityId)]
    public async Task<ActionResult<GetCityResponse>> GetCity([FromRoute] Guid id)
    {
        return await Mediator.Send(new GetCityQuery { Id = id });
    }

    [HttpGet(ApiEndpoint.V1.Cities.RouteTemplateFor.CityForUser)]
    [AllowAnonymous]
    public async Task<ActionResult<ListResponse<GetCities_CityForUser>>> GetCitiesForUser()
    {
        return await Mediator.Send(new GetCitiesForUserQuery());
    }
}
