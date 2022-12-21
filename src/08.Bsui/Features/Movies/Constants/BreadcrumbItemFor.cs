using MudBlazor;
using Zeta.NontonFilm.Shared.Movies.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Movies.Constants;

public class BreadcrumbItemFor
{
    public static readonly BreadcrumbItem Index = new(DisplayTextFor.Movies, RouteFor.Index);

    public static BreadcrumbItem Details(Guid id, string text)
    {
        return new(text, RouteFor.Details(id));
    }
}

