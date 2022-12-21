using MudBlazor;
using Zeta.NontonFilm.Shared.Cinemas.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Cinemas.Constants;

public class BreadcrumbItemFor
{
    public static readonly BreadcrumbItem CinemaIndex = new(DisplayTextFor.CinemaChains, RouteFor.Index);

    public static BreadcrumbItem Details(Guid id, string text)
    {
        return new(text, RouteFor.Details(id));
    }

    public static BreadcrumbItem CinemaChainDetails(Guid id, string text)
    {
        return new(text, RouteFor.CinemaChainDetails(id));
    }
}

