using Zeta.NontonFilm.Shared.Services.HealthCheck.Constants;
using Zeta.NontonFilm.Shared.Services.HealthCheck.Queries.GetHealthCheck;
using RestSharp;

namespace Zeta.NontonFilm.Client.Services.HealthCheck;

public class HealthCheckService
{
    private readonly RestClient _restClient;

    public HealthCheckService()
    {
        _restClient = new RestClient();
    }

    public async Task<GetHealthCheckResponse> GetHealthCheckAsync(string healthCheckUrl)
    {
        var restRequest = new RestRequest(healthCheckUrl, Method.Get);
        var restResponse = await _restClient.ExecuteAsync<GetHealthCheckResponse>(restRequest);

        if (restResponse.Data is not null)
        {
            return restResponse.Data;
        }

        if (!restResponse.IsSuccessful)
        {
            return new GetHealthCheckResponse
            {
                Status = HealthCheckStatus.Unhealthy
            };
        }

        return new GetHealthCheckResponse
        {
            Status = HealthCheckStatus.Unknown
        };
    }
}
