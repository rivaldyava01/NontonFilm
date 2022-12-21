using Newtonsoft.Json;

namespace Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde.Models.GetPositions;

public class GetPositionsZetaGardePersona
{
    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("positions")]
    public GetPositionsZetaGardePosition[] Positions { get; set; } = default!;
}
