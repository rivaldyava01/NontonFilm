using MudBlazor;
using Zeta.NontonFilm.Shared.Movies.Constants;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets.Constants;

public class BreadcrumbItemFor
{
    public static BreadcrumbItem Index(Guid id)
    {
        return new(DisplayTextFor.Movies, RouteFor.Index(id));
    }
}

