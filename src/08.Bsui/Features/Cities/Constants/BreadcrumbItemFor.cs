using MudBlazor;
using Zeta.NontonFilm.Shared.Cities.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Cities.Constants;

public class BreadcrumbItemFor
{
    public static readonly BreadcrumbItem Index = new(DisplayTextFor.Cities, RouteFor.Index);

    public static BreadcrumbItem Details(Guid id, string text)
    {
        return new(text, RouteFor.Details(id));
    }
}

