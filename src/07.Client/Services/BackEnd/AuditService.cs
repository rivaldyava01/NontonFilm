using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Audits.Queries.ExportAudits;
using Zeta.NontonFilm.Shared.Audits.Queries.GetAudit;
using Zeta.NontonFilm.Shared.Audits.Queries.GetAudits;
using Zeta.NontonFilm.Shared.Common.Responses;
using static Zeta.NontonFilm.Shared.Audits.Constants.ApiEndpoint.V1;

namespace Zeta.NontonFilm.Client.Services.BackEnd;

public class AuditService
{
    private readonly RestClient _restClient;

    public AuditService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}/{Audits.Segment}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<PaginatedListResponse<GetAuditsAudit>>> GetAuditsAsync(GetAuditsRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Get);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetAuditsAudit>>();
    }

    public async Task<ResponseResult<GetAuditResponse>> GetAuditAsync(Guid auditId)
    {
        var restRequest = new RestRequest(auditId.ToString(), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetAuditResponse>();
    }

    public async Task<ResponseResult<ExportAuditsResponse>> ExportAuditsAsync(ExportAuditsRequest request)
    {
        var restRequest = new RestRequest(nameof(Audits.RouteTemplateFor.Export), Method.Get);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ExportAuditsResponse>();
    }
}
