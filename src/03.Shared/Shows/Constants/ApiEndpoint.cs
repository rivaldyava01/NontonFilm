namespace Zeta.NontonFilm.Shared.Shows.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Shows
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Shows)}";

            public static class RouteTemplateFor
            {
                public const string ShowId = "{Id:guid}";
                public const string PastShow = "past/{StudioId:guid}";
                public const string UpcomingShow = "upcoming/{StudioId:guid}";
                public const string StudioId = "studio/{studioId:guid}";
            }
        }
    }
}
