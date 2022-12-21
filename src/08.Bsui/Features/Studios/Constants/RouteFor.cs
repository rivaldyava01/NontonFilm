namespace Zeta.NontonFilm.Bsui.Features.Studios.Constants;

public static class RouteFor
{
    public const string CinemaIndex = nameof(CinemaChains);

    public static string Details(Guid id)
    {
        return $"{nameof(Studios)}/{nameof(Details)}/{id}";
    }

    public static string CinemaDetails(Guid id)
    {
        return $"{nameof(Cinemas)}/{nameof(Details)}/{id}";
    }

    public static string CinemaChainDetails(Guid id)
    {
        return $"{nameof(CinemaChains)}/{nameof(Details)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(Studios)}/{nameof(Edit)}/{id}";
    }
}
