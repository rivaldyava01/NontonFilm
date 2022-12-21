using Zeta.NontonFilm.Shared.Services.HealthCheck.Constants;

namespace Zeta.NontonFilm.Shared.Services.HealthCheck.Queries.GetHealthCheck;

public class GetHealthCheckResponse
{
    public string Status { get; set; } = HealthCheckStatus.Loading;
    public TimeSpan TotalDuration { get; set; }
    public IDictionary<string, GetHealthCheckHealthCheckEntry> Entries { get; set; } = new Dictionary<string, GetHealthCheckHealthCheckEntry>();
}
