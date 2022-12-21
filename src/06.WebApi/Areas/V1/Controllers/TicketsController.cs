using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Tickets.Commands.AddTicketSales;
using Zeta.NontonFilm.Application.Tickets.Queries.GetTicketQrCode;
using Zeta.NontonFilm.Application.Tickets.Queries.GetTickets;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Tickets.Constants;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketQrCode;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTickets;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketTransactionHistoriesBuUserid;
using Zeta.NontonFilm.WebApi.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class TicketsController : ApiControllerBase
{
    [HttpPost]
    [Consumes(RequestContentTypes.Form)]
    public async Task<ActionResult<ItemCreatedResponse>> AddTicketSales([FromForm] AddTicketSalesCommand request)
    {
        var response = await Mediator.Send(request);

        return CreatedAtAction(nameof(AddTicketSales), response);
    }

    [HttpGet(ApiEndpoint.V1.Tickets.RouteTemplateFor.TicketList)]
    public async Task<ActionResult<ListResponse<GetTickets_Ticket>>> GetTickets([FromRoute] Guid showId)
    {
        return await Mediator.Send(new GetTicketsQuery { ShowId = showId });
    }

    [HttpGet(ApiEndpoint.V1.Tickets.RouteTemplateFor.TrasactionHistory)]
    public async Task<ActionResult<ListResponse<GetTicketTransactionHistoriesbyUserIdResponse>>> GetTicketTransactionHistoriesByUserId()
    {
        return await Mediator.Send(new GetTicketTransactionHistoriesByUserIdQuery());
    }

    [HttpGet(ApiEndpoint.V1.Tickets.RouteTemplateFor.QrCode)]
    [Produces(typeof(GetTicketQrCodeResponse))]
    public async Task<ActionResult<GetTicketQrCodeResponse>> GetTicketQrCode([FromRoute] Guid ticketSalesId)
    {
        return await Mediator.Send(new GetTicketQrCodeQuery { TicketSalesId = ticketSalesId });
    }
}
