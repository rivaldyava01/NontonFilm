using Newtonsoft.Json;

namespace Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde.Models.GetPositions;

public class GetPositionsZetaGardePosition
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}
