using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Pages.Account.Constants;
using Zeta.NontonFilm.Bsui.Services.Authorization.Components;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;

namespace Zeta.NontonFilm.Bsui.Services.Authentication.Components;

public partial class AccountInfo
{
    private void NavigateToMySession()
    {
        _navigationManager.NavigateTo(CommonRouteFor.MySession, true);
    }

    private async Task ShowDialogSwitchPosition()
    {
        var dialog = _dialogService.Show<DialogSwitchPosition>(AuthenticationDisplayTextFor.SwitchPosition);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            StateHasChanged();
        }
    }

    private void NavigateToLogout()
    {
        _navigationManager.NavigateTo(RouteFor.Logout, true);
    }
}
