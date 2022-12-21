using MudBlazor;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Bsui.Common.Constants;

public static class CommonBreadcrumbFor
{
    public static BreadcrumbItem Active(string text)
    {
        return new(text, href: null, disabled: true);
    }

    public static readonly BreadcrumbItem Home = new(CommonDisplayTextFor.Home, href: CommonRouteFor.Index);
}
