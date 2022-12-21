using Newtonsoft.Json;

namespace Zeta.NontonFilm.Infrastructure.Authorization.ZetaGarde.Models.GetAuhtorizationInfo;

public class GetAuhtorizationInfoZetaGardeApplication
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}
