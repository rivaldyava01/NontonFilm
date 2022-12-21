using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Genres.Commands.AddGenre;
using Zeta.NontonFilm.Shared.Genres.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Genres.Components;

public partial class DialogAdd
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    private readonly AddGenreRequest _request = new();

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

        var responseResult = await _genreService.AddGenreAsync(_request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        if (responseResult.Result is not null)
        {
            _isLoading = false;

            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Add.ToLower()} {DisplayTextFor.Genre} {_request.Name}", Severity.Success);

            MudDialog.Close(DialogResult.Ok(responseResult.Result.Id));
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
