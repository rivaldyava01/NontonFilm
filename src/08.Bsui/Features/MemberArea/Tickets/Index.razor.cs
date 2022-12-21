using Microsoft.AspNetCore.Components;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets.Components;
using Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Tickets.Commands.AddTicketSales;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets;

public partial class Index
{
    [Parameter]
    public Guid ShowId { get; set; }

    private readonly List<Seats> _seats = new();
    private readonly List<List<Seats>> _seats2D = new();
    private readonly TicketSell _ticketSell = new();
    private Guid StudioId { get; set; }
    private decimal TicketPrice { get; set; }
    private int Rows { get; set; }
    private int Columns { get; set; }
    private ErrorResponse? _error;

    protected override async Task OnInitializedAsync()
    {
        await ReloadShow();
        await ReloadTicket();
    }

    private async Task ReloadShow()
    {
        var response = await _showService.GetShowAsync(ShowId);

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        if (response.Result is not null)
        {
            StudioId = response.Result.StudioId;
            TicketPrice = response.Result.TicketPrice;
            _ticketSell.MovieTitle = response.Result.MovieTitle;
            _ticketSell.ShowDateTime = response.Result.ShowDateTime;
            _ticketSell.StudioName = response.Result.StudioName;
            _ticketSell.ShowId = ShowId;
        }
    }

    private async Task ReloadTicket()
    {
        await LoadAllSeatsFromWebApi();
        var responseTicket = await _ticketService.GetTickets(ShowId);

        if (responseTicket.Error is not null)
        {
            _error = responseTicket.Error;

            return;
        }

        if (responseTicket.Result is not null)
        {

            for (var i = 1; i <= Rows; i++)
            {
                var columnSeats = new List<Seats>();

                for (var j = 1; j <= Columns; j++)
                {
                    var seat = new Seats
                    {
                        Row = i,
                        Column = j
                    };

                    columnSeats.Add(seat);
                    _seats.Add(seat);
                }

                _seats2D.Add(columnSeats);
            }

            foreach (var ticket in responseTicket.Result.Items)
            {
                foreach (var seat in _seats)
                {
                    if (seat.Code == ticket.SeatCode)
                    {
                        seat.Id = ticket.SeatId;
                        seat.TicketSalesId = ticket.TicketSalesId;
                    }
                }
            }
        }
    }

    private async Task LoadAllSeatsFromWebApi()
    {
        var response = await _seatService.GetSeatsAsync(StudioId);

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        if (response.Result is not null)
        {
            Rows = response.Result.Row;
            Columns = response.Result.Column;
        }
    }

    private static void ToggleChosen(Seats seat)
    {
        seat.IsChosen = !seat.IsChosen;
    }

    private async Task ShowDialogConfirmationPurchaseTicket()
    {
        var chosenSeats = _seats.Where(x => x.IsChosen).Select(y => y.Code);
        var chosenSeatsSell = string.Join(", ", chosenSeats);

        _ticketSell.SeatCodes = chosenSeatsSell;
        _ticketSell.TicketPrice = TicketPrice * chosenSeats.Count();

        var dialogTitle = $"Confirmation Purchase Ticket";

        var request = _ticketSell;

        var dialogParameters = new DialogParameters
        {
            { nameof(DialogConfirmationPurchaseTicket.Request), request }
        };

        var dialog = _dialogService.Show<DialogConfirmationPurchaseTicket>(dialogTitle, dialogParameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var ticketSalesRequest = new AddTicketSalesRequest { ShowId = ShowId };

            foreach (var seat in chosenSeats)
            {
                ticketSalesRequest.SeatCode.Add(seat);
            }

            var responseResult = await _ticketService.AddTicketSalesAsync(ticketSalesRequest);

            if (responseResult.Error is not null)
            {
                _error = responseResult.Error;

                return;
            }

            if (responseResult.Result is not null)
            {
                _snackbar.Add($"Tickets to be sold: {_ticketSell.SeatCodes}", Severity.Success);

                _navigationManager.NavigateTo(RouteFor.TransactionHistory);
            }
        }

    }
}

public class Seats
{
    public Guid Id { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public string Code => $"{Convert.ToChar(64 + Row)}{Column}";
    public bool IsChosen { get; set; } = false;
    public Guid? TicketSalesId { get; set; }
}

public class TicketSell
{
    public Guid ShowId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public string StudioName { get; set; } = default!;
    public DateTime ShowDateTime { get; set; }
    public decimal TicketPrice { get; set; }
    public string SeatCodes { get; set; } = default!;
}
