namespace Zeta.NontonFilm.Shared.Movies.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Movies
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Movies)}";

            public static class RouteTemplateFor
            {
                public const string MovieId = "{Id:guid}";
                public const string List = nameof(List);
                public const string NowShowing = "nowshowing";
                public const string MovieNowShowing = "nowshowing/{Id:guid}";
            }
        }
    }
}
