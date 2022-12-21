using Microsoft.AspNetCore.Components.Forms;
using Zeta.NontonFilm.Bsui.Features.CinemaChains.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.CinemaChains.Commands.AddCinemaChain;

namespace Zeta.NontonFilm.Bsui.Features.CinemaChains;

public partial class Add
{
    private bool _isLoading;
    private ErrorResponse? _error;
    private readonly AddCinemaChainRequest _request = new();

    private async Task OnValidSubmit()
    {
        _isLoading = true;

        _error = null;

        var responseResult = await _cinemaChainService.AddCinemaChainAsync(_request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Berhasil menambahkan CinemaChain. ID: {responseResult.Result.Id}", MudBlazor.Severity.Success);
            _navigationManager.NavigateTo(RouteFor.Index);
        }

        _isLoading = false;
    }

    private void OnInvalidSubmit(EditContext editContext)
    {
        foreach (var validationMessage in editContext.GetValidationMessages())
        {
            _snackbar.Add(validationMessage, MudBlazor.Severity.Error);
        }
    }
}
