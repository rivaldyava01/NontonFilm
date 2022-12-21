using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Zeta.NontonFilm.Bsui.Common.Components;

public partial class DialogDelete
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private void Delete()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }
}
