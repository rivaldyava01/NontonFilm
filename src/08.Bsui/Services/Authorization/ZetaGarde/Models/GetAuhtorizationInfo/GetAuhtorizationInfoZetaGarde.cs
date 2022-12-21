using Newtonsoft.Json;

namespace Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde.Models.GetAuhtorizationInfo;

public class GetAuhtorizationInfoZetaGarde
{
    [JsonProperty("application")]
    public GetAuhtorizationInfoZetaGardeApplication Application { get; set; } = default!;

    [JsonProperty("roles")]
    public GetAuhtorizationInfoZetaGardeRole[] Roles { get; set; } = Array.Empty<GetAuhtorizationInfoZetaGardeRole>();

    [JsonProperty("customParameters")]
    public IEnumerable<IDictionary<string, string>> CustomParameters { get; set; } = default!;
}

