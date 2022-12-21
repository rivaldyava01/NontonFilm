namespace Zeta.NontonFilm.Bsui.Features.Movies.Constants;

public static class RouteFor
{
    public const string Index = nameof(Movies);

    public static string Details(Guid id)
    {
        return $"{nameof(Movies)}/{nameof(Details)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(Movies)}/{nameof(Edit)}/{id}";
    }
}
