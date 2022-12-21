namespace Zeta.NontonFilm.Bsui.Features.Genres.Constants;

public static class RouteFor
{
    public const string Index = nameof(Genres);

    public static string Details(Guid id)
    {
        return $"{nameof(Genres)}/{nameof(Details)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(Genres)}/{nameof(Edit)}/{id}";
    }
}
