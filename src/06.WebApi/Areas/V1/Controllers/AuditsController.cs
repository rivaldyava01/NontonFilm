using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Application.Audits.Queries.ExportAudits;
using Zeta.NontonFilm.Application.Audits.Queries.GetAudit;
using Zeta.NontonFilm.Application.Audits.Queries.GetAudits;
using Zeta.NontonFilm.Shared.Audits.Constants;
using Zeta.NontonFilm.Shared.Audits.Queries.ExportAudits;
using Zeta.NontonFilm.Shared.Audits.Queries.GetAudit;
using Zeta.NontonFilm.Shared.Audits.Queries.GetAudits;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.WebApi.Common.Extensions;

namespace Zeta.NontonFilm.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class AuditsController : ApiControllerBase
{
    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetAuditsAudit>))]
    public async Task<ActionResult<PaginatedListResponse<GetAuditsAudit>>> GetAudits([FromQuery] GetAuditsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet(ApiEndpoint.V1.Audits.RouteTemplateFor.AuditId)]
    [Produces(typeof(GetAuditResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetAuditResponse>> GetAudit([FromRoute] Guid auditId)
    {
        return await Mediator.Send(new GetAuditQuery { AuditId = auditId });
    }

    [HttpGet(ApiEndpoint.V1.Audits.RouteTemplateFor.Export)]
    [Produces(typeof(ExportAuditsResponse))]
    public async Task<ActionResult> ExportAudits([FromQuery] IList<Guid> auditIds)
    {
        var query = new ExportAuditsQuery
        {
            AuditIds = auditIds
        };

        var response = await Mediator.Send(query);

        return response.AsFile();
    }
}
