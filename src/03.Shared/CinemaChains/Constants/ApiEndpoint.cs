namespace Zeta.NontonFilm.Shared.CinemaChains.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class CinemaChains
        {
            public const string Segment = $"{nameof(V1)}/{nameof(CinemaChains)}";

            public static class RouteTemplateFor
            {
                public const string CinemaChainId = "{Id:guid}";
                public const string CinemaChainList = "list";
            }
        }
    }
}
