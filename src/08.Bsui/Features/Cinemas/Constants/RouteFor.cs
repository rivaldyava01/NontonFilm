using Zeta.NontonFilm.Shared.Cinemas.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Cinemas.Constants;

public static class RouteFor
{
    public const string Index = nameof(Cinemas);

    public static string Details(Guid id)
    {
        return $"{nameof(Cinemas)}/{nameof(Details)}/{id}";
    }

    public static string CinemaChainDetails(Guid id)
    {
        return $"{nameof(CinemaChains)}/{nameof(Details)}/{id}";
    }

    public static string StudioDetails(Guid id)
    {
        return $"{DisplayTextFor.Studios}/{nameof(Details)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(Cinemas)}/{nameof(Edit)}/{id}";
    }
}
