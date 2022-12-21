using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Features.Cities.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Cities.Commands.UpdateCity;

namespace Zeta.NontonFilm.Bsui.Features.Cities.Components;

public partial class DialogEdit
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public UpdateCityRequest Request { get; set; } = default!;

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

        var responseResult = await _cityService.UpdateCityAsync(Request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        _isLoading = false;

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Berhasil update City. ID: {Request.Id}", Severity.Success);

            _navigationManager.NavigateTo(RouteFor.Index);
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
