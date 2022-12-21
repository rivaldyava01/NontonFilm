using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeta.NontonFilm.Shared.Services.HealthCheck.Constants;
using Zeta.NontonFilm.Shared.Services.HealthCheck.Queries.GetHealthCheck;
using RestSharp;

namespace Zeta.NontonFilm.Infrastructure.Authorization.ZetaGarde;

public class ZetaGardeAuthorizationHealthCheck : IHealthCheck
{
    private readonly RestClient _restClient;

    public ZetaGardeAuthorizationHealthCheck(string healthCheckUrl)
    {
        _restClient = new RestClient(healthCheckUrl);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest(string.Empty, Method.Get);
            var restResponse = await _restClient.ExecuteAsync<GetHealthCheckResponse>(restRequest, cancellationToken);
            var uri = _restClient.BuildUri(restRequest).AbsoluteUri;
            var descriptionPrefix = $"Response from {uri} is ";

            if (!restResponse.IsSuccessful)
            {
                return HealthCheckResult.Unhealthy($"{descriptionPrefix} {restResponse.ErrorMessage}");
            }

            if (restResponse.Data is null)
            {
                return HealthCheckResult.Unhealthy($"{descriptionPrefix} {restResponse.Content}");
            }

            var fullDescription = $"{descriptionPrefix} {restResponse.Data.Status}";

            return restResponse.Data.Status switch
            {
                HealthCheckStatus.Unhealthy => HealthCheckResult.Unhealthy(fullDescription),
                HealthCheckStatus.Degraded => HealthCheckResult.Degraded(fullDescription),
                HealthCheckStatus.Healthy => HealthCheckResult.Healthy(fullDescription),
                HealthCheckStatus.Loading => HealthCheckResult.Unhealthy(fullDescription),
                _ => HealthCheckResult.Unhealthy(fullDescription),
            };
        }
        catch (Exception exception)
        {
            return context.Registration.FailureStatus is HealthStatus.Unhealthy
                ? HealthCheckResult.Unhealthy(exception.Message)
                : HealthCheckResult.Degraded(exception.Message);
        }
    }
}
