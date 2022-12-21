using MudBlazor;
using Zeta.NontonFilm.Shared.Genres.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Genres.Constants;

public class BreadcrumbItemFor
{
    public static readonly BreadcrumbItem Index = new(DisplayTextFor.Genres, RouteFor.Index);

    public static BreadcrumbItem Details(Guid id, string text)
    {
        return new(text, RouteFor.Details(id));
    }
}

