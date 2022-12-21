using MudBlazor;
using Zeta.NontonFilm.Shared.Audits.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Audits.Constants;

public static class BreadcrumbFor
{
    public static readonly BreadcrumbItem Index = new(DisplayTextFor.Audits, href: RouteFor.Index);
}
