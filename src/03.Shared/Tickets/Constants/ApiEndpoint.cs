namespace Zeta.NontonFilm.Shared.Tickets.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Tickets
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Tickets)}";

            public static class RouteTemplateFor
            {
                public const string ShowId = "{ShowId:guid}";
                public const string TrasactionHistory = "user/transactionhistory";
                public const string TicketList = "list/{ShowId:guid}";
                public const string QrCode = "user/qrcode/{TicketSalesId:guid}";
            }
        }
    }
}
