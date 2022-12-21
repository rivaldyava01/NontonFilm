namespace Zeta.NontonFilm.Shared.Cinemas.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Cinemas
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Cinemas)}";

            public static class RouteTemplateFor
            {
                public const string CinemaId = "{Id:guid}";
                public const string CinemaChainId = "cinemachain/{CinemaChainId:guid}";
                public const string CityId = "city/{CityId:guid}";
                public const string MovieId = "movie/{MovieId:guid}";
                public const string CinemaList = "list";
            }
        }
    }
}
