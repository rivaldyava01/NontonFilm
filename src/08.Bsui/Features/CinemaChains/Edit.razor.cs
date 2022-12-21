using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Features.CinemaChains.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.CinemaChains.Commands.UpdateCinemaChain;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Bsui.Features.CinemaChains;

public partial class Edit
{
    [Parameter]
    public Guid CinemaChainId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private UpdateCinemaChainRequest? _request;
    private bool _isLoading;
    private ErrorResponse? _error;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.Index);
    }

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        var responseResult = await _cinemaChainService.GetCinemaChainAsync(CinemaChainId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        if (responseResult.Result is not null)
        {
            var cinemaChain = responseResult.Result;

            _request = new UpdateCinemaChainRequest
            {
                Id = CinemaChainId,
                Name = cinemaChain.Name,
                OfficeAddress = cinemaChain.OfficeAddress,
                PhoneNumber = cinemaChain.PhoneNumber,
                EmailAddress = cinemaChain.EmailAddress
            };

            _breadcrumbItems.Add(BreadcrumbItemFor.Details(cinemaChain.Id, cinemaChain.Name));
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Edit));
        }

        _isLoading = false;
    }

    private async Task OnValidSubmit()
    {
        if (_request is null)
        {
            return;
        }

        _isLoading = true;

        _error = null;

        var responseResult = await _cinemaChainService.UpdateCinemaChainAsync(_request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        _isLoading = false;

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Berhasil update CinemaChain. ID: {CinemaChainId}", Severity.Success);

            _navigationManager.NavigateTo(RouteFor.Details(CinemaChainId));
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
