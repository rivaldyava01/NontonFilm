using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Seats.Queries.GetSeat;
using Zeta.NontonFilm.Application.Seats.Queries.GetSeats;
using Zeta.NontonFilm.Shared.Seats.Constants;
using Zeta.NontonFilm.Shared.Seats.Queries.GetSeat;
using Zeta.NontonFilm.Shared.Seats.Queries.GetSeats;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class SeatsController : ApiControllerBase
{

    [HttpGet(ApiEndpoint.V1.Seats.RouteTemplateFor.TotalSeat)]
    [AllowAnonymous]
    public async Task<ActionResult<GetSeatsResponse>> GetSeats([FromRoute] Guid studioId)
    {
        return await Mediator.Send(new GetSeatsQuery { StudioId = studioId });
    }

    [HttpGet(ApiEndpoint.V1.Seats.RouteTemplateFor.Seat)]
    [AllowAnonymous]
    public async Task<ActionResult<GetSeatResponse>> GetSeat([FromRoute] Guid studioId, [FromForm] GetSeatRequest request)
    {
        if (studioId != request.StudioId && request.StudioId is not null)
        {
            throw new MismatchException(nameof(GetSeatResponse.Id), studioId, request.StudioId);
        }

        Console.WriteLine(request.StudioId);
        var query = new GetSeatQuery
        {
            StudioId = studioId,
            Row = request.Row,
            Column = request.Column
        };

        return await Mediator.Send(query);
    }
}
