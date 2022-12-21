using MudBlazor;
using Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets.Components;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Tickets.Constants;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketQrCode;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketTransactionHistoriesBuUserid;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets;

public partial class TransactionHistory
{
    private ErrorResponse? _error;
    private List<GetTicketTransactionHistoriesbyUserIdResponse> _transactionHistory = new();
    private GetTicketQrCodeResponse QrCode { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {

        var responseResult = await _ticketService.GetTicketTransactionHistoriesByUserId();

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            foreach (var transactionHistory in responseResult.Result.Items)
            {
                _transactionHistory.Add(new GetTicketTransactionHistoriesbyUserIdResponse
                {
                    TicketSalesId = transactionHistory.TicketSalesId,
                    MovieTitle = transactionHistory.MovieTitle,
                    MoviePosterImage = transactionHistory.MoviePosterImage,
                    CinemaName = transactionHistory.CinemaName,
                    DateShow = transactionHistory.DateShow,
                    TimeShow = transactionHistory.TimeShow,
                    Created = transactionHistory.Created,
                    StudioName = transactionHistory.StudioName,
                    SeatCode = transactionHistory.SeatCode
                });
            }
        }

        _transactionHistory = _transactionHistory.OrderByDescending(x => x.Created).ToList();
    }

    private async Task ShowDialogViewQrCode(Guid ticketSalesId)
    {
        var response = await _ticketService.GetTicketQrCode(ticketSalesId);

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        QrCode = response.Result!;

        var parameters = new DialogParameters
        {
            { nameof(DialogViewTicketQrCode.Data), QrCode.Data }
        };

        var options = new DialogOptions
        {
            CloseButton = true
        };

        _dialogService.Show<DialogViewTicketQrCode>($"{DisplayTextFor.QrCode} for your Ticket", parameters, options);
    }
}
