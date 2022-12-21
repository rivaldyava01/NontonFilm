using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets.Components;

public partial class DialogConfirmationPurchaseTicket
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public TicketSell Request { get; set; } = default!;

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(Request.MovieTitle));
    }
}
