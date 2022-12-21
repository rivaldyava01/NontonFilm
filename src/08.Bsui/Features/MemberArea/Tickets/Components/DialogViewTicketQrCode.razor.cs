using Microsoft.AspNetCore.Components;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets.Components;

public partial class DialogViewTicketQrCode
{
    [Parameter]
    public string Data { get; set; } = default!;
}
