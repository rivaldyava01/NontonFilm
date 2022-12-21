using Zeta.NontonFilm.Shared.CinemaChains.Constants;

namespace Zeta.NontonFilm.Bsui.Features.CinemaChains.Constants;

public static class RouteFor
{
    public const string Index = nameof(CinemaChains);

    public static string Details(Guid id)
    {
        return $"{nameof(CinemaChains)}/{nameof(Details)}/{id}";
    }

    public static string CinemaDetails(Guid id)
    {
        return $"{DisplayTextFor.Cinemas}/{nameof(Details)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(CinemaChains)}/{nameof(Edit)}/{id}";
    }
}
