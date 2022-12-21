using MudBlazor;
using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Studios.Constants;

public class BreadcrumbItemFor
{
    public static readonly BreadcrumbItem CinemaIndex = new(DisplayTextFor.CinemaChains, RouteFor.CinemaIndex);

    public static BreadcrumbItem Details(Guid id, string text)
    {
        return new(text, RouteFor.Details(id));
    }

    public static BreadcrumbItem CinemaChainDetails(Guid id, string text)
    {
        return new(text, RouteFor.CinemaChainDetails(id));
    }

    public static BreadcrumbItem CinemaDetails(Guid id, string text)
    {
        return new(text, RouteFor.CinemaDetails(id));
    }
}

