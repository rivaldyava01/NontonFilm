using Newtonsoft.Json;

namespace Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde.Models.GetAuhtorizationInfo;

public class GetAuhtorizationInfoZetaGardeApplication
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}
