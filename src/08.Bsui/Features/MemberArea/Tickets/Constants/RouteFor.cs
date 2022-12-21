namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Tickets.Constants;

public static class RouteFor
{
    public const string TransactionHistory = $"{nameof(Tickets)}/transactionhistory/user";

    public static string Index(Guid id)
    {
        return $"{nameof(Tickets)}/User/{id}";
    }
}
