using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;

namespace Zeta.NontonFilm.Bsui.Common.Pages;

public partial class MySession
{
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudMessageBox? _messageBoxAccessToken;
    private bool _inProductionEnvironment;
    private string _accessToken = default!;

    protected override void OnInitialized()
    {
        SetupBreadcrumb();

        _inProductionEnvironment = _webHostEnvironment.IsInEnvironment(EnvironmentNames.Production);
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(AuthenticationDisplayTextFor.MySession)
        };
    }

    private async void ShowMessageBoxAccessToken()
    {
        await _messageBoxAccessToken!.Show();
    }
}
