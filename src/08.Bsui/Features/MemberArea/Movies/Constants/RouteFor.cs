namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Movies.Constants;

public static class RouteFor
{
    public const string Index = $"{nameof(Movies)}/user/{nameof(NowShowing)}";

    public static string Details(Guid id)
    {
        return $"{nameof(Movies)}/user/{nameof(NowShowing)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(Movies)}/{nameof(Edit)}/{id}";
    }

    public static string TicketDetails(Guid id)
    {
        return $"{nameof(Tickets)}/User/{id}";
    }
}
