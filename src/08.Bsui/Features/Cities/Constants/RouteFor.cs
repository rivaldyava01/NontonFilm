namespace Zeta.NontonFilm.Bsui.Features.Cities.Constants;

public static class RouteFor
{
    public const string Index = nameof(Cities);

    public static string Details(Guid id)
    {
        return $"{nameof(Cities)}/{nameof(Details)}/{id}";
    }

    public static string CinemaDetails(Guid id)
    {
        return $"{nameof(Cinemas)}/{nameof(Details)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(Cities)}/{nameof(Edit)}/{id}";
    }
}
