using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Features.Genres.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Genres.Commands.UpdateGenre;
using Zeta.NontonFilm.Shared.Genres.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Genres.Components;

public partial class DialogEdit
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public UpdateGenreRequest Request { get; set; } = default!;

    private bool _isLoading;
    private ErrorResponse? _error;

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task OnValidSubmit()
    {
        _isLoading = true;

        _error = null;

        var responseResult = await _genreService.UpdateGenreAsync(Request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        _isLoading = false;

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Update.ToLower()} {DisplayTextFor.Genre} {Request.Name}", Severity.Success);
            MudDialog.Close(DialogResult.Ok(Request.Name));
        }
    }

    private void OnInvalidSubmit(EditContext editContext)
    {
        foreach (var validationMessage in editContext.GetValidationMessages())
        {
            _snackbar.Add(validationMessage, Severity.Error);
        }
    }
}
