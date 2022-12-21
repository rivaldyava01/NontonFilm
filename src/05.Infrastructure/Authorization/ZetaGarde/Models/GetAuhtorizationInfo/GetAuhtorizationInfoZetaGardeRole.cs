using Newtonsoft.Json;

namespace Zeta.NontonFilm.Infrastructure.Authorization.ZetaGarde.Models.GetAuhtorizationInfo;

public class GetAuhtorizationInfoZetaGardeRole
{
    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("permissions")]
    public string[]? Permissions { get; set; }
}
