namespace Zeta.NontonFilm.Shared.Cities.Constants;

public static class ApiEndpoint
{
    public static class V1
    {
        public static class Cities
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Cities)}";

            public static class RouteTemplateFor
            {
                public const string CityId = "{Id:guid}";
                public const string CityList = "list";
                public const string CityForUser = "user/list";
            }
        }
    }
}
