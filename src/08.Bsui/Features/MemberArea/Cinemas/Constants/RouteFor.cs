namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Cinemas.Constants;

public static class RouteFor
{
    public const string Index = $"{nameof(Cinemas)}/user";

    public static string TicketDetails(Guid id)
    {
        return $"{nameof(Tickets)}/User/{id}";
    }

    public static string Details(Guid id)
    {
        return $"{nameof(Movies)}/{nameof(Details)}/{id}";
    }

    public static string Edit(Guid id)
    {
        return $"{nameof(Movies)}/{nameof(Edit)}/{id}";
    }
}
