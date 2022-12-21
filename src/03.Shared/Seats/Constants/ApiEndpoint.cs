namespace Zeta.NontonFilm.Shared.Seats.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Seats
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Seats)}";

            public static class RouteTemplateFor
            {
                public const string TotalSeat = "total/{StudioId:guid}";
                public const string Seat = "{StudioId:guid}";
            }
        }
    }
}
