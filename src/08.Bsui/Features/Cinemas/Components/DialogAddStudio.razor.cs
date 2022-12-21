using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Studios.Commands.AddStudio;
using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Cinemas.Components;

public partial class DialogAddStudio
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid CinemaId { get; set; }

    private readonly AddStudioCommand _request = new();

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
        var request = new AddStudioRequest
        {
            Name = _request.Name,
            CinemaId = CinemaId,
            Column = _request.Column,
            Row = _request.Row
        };

        var responseResult = await _studioService.AddStudioAsync(request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        if (responseResult.Result is not null)
        {
            _isLoading = false;

            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Add.ToLower()} {DisplayTextFor.Studio} {_request.Name}", Severity.Success);

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

public class AddStudioCommand
{
    public string Name { get; set; } = default!;
    public Guid CinemaId { get; set; } = default!;
    public int Row { get; set; } = default!;
    public int Column { get; set; } = default!;
}

