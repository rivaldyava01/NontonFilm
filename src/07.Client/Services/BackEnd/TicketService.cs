using Microsoft.Extensions.Options;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Client.Services.UserInfo;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Tickets.Commands.AddTicketSales;
using Zeta.NontonFilm.Shared.Tickets.Constants;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketQrCode;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTickets;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketTransactionHistoriesBuUserid;

namespace Zeta.NontonFilm.Client.Services.BackEnd;
public class TicketService
{
    private readonly RestClient _restClient;

    public TicketService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<ItemCreatedResponse>> AddTicketSalesAsync(AddTicketSalesRequest request)
    {
        var restRequest = new RestRequest(ApiEndpoint.V1.Tickets.Segment, Method.Post);

        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ItemCreatedResponse>();
    }

    public async Task<ResponseResult<ListResponse<GetTickets_Ticket>>> GetTickets(Guid showId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Tickets.Segment}/list/{showId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);
        return restResponse.ToResponseResult<ListResponse<GetTickets_Ticket>>();
    }

    public async Task<ResponseResult<ListResponse<GetTicketTransactionHistoriesbyUserIdResponse>>> GetTicketTransactionHistoriesByUserId()
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Tickets.Segment}/user/transactionhistory", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);
        return restResponse.ToResponseResult<ListResponse<GetTicketTransactionHistoriesbyUserIdResponse>>();
    }

    public async Task<ResponseResult<GetTicketQrCodeResponse>> GetTicketQrCode(Guid tikcetSalesId)
    {
        var restRequest = new RestRequest($"{ApiEndpoint.V1.Tickets.Segment}/user/qrcode/{tikcetSalesId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);
        return restResponse.ToResponseResult<GetTicketQrCodeResponse>();
    }
}
