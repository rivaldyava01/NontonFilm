using MudBlazor;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;

namespace Zeta.NontonFilm.Bsui.Features.CinemaChains.Constants;

public class BreadcrumbItemFor
{
    public static readonly BreadcrumbItem Index = new(DisplayTextFor.CinemaChains, RouteFor.Index);

    public static BreadcrumbItem Details(Guid id, string text)
    {
        return new(text, RouteFor.Details(id));
    }
}

