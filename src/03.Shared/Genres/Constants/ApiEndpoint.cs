namespace Zeta.NontonFilm.Shared.Genres.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Genres
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Genres)}";

            public static class RouteTemplateFor
            {
                public const string GenreId = "{Id:guid}";
            }
        }
    }
}
