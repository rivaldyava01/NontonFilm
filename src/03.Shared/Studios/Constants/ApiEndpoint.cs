namespace Zeta.NontonFilm.Shared.Studios.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Studios
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Studios)}";

            public static class RouteTemplateFor
            {
                public const string StudioId = "{Id:guid}";
                public const string CinemaId = "cinema/{CinemaId:guid}";
            }
        }
    }
}
