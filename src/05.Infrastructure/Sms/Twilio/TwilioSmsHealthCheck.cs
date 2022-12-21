using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace Zeta.NontonFilm.Infrastructure.Sms.Twilio;

public class TwilioSmsHealthCheck : IHealthCheck
{
    private readonly RestClient _restClient;

    public TwilioSmsHealthCheck(string healthCheckUrl)
    {
        _restClient = new RestClient(healthCheckUrl);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest(string.Empty, Method.Get);
            var restResponse = await _restClient.ExecuteAsync<TwilioSmsHealthCheckResult>(restRequest, cancellationToken);
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

            Console.WriteLine($"restResponse.Content from {nameof(Twilio)} {restResponse.Content}");

            var id = restResponse.Data.Page.Id;
            var name = restResponse.Data.Page.Name;
            var url = restResponse.Data.Page.Url;
            var timeZone = restResponse.Data.Page.TimeZone;
            var updated = restResponse.Data.Page.Updated;
            var indicator = restResponse.Data.Status.Indicator;
            var description = restResponse.Data.Status.Description;

            if (indicator == "none")
            {
                return HealthCheckResult.Healthy(description);
            }
            else if (indicator is "maintenance" or "minor")
            {
                var fullDescription = string.IsNullOrWhiteSpace(timeZone)
                    ? description
                    : $"{description} in {timeZone}";

                return HealthCheckResult.Degraded(fullDescription);
            }
            else
            {
                return HealthCheckResult.Unhealthy(description);
            }
        }
        catch (Exception exception)
        {
            return context.Registration.FailureStatus is HealthStatus.Unhealthy
                ? HealthCheckResult.Unhealthy(exception.Message)
                : HealthCheckResult.Degraded(exception.Message);
        }
    }
}

public class TwilioSmsHealthCheckResult
{
    [JsonPropertyName("page")]
    public Page Page { get; set; } = default!;

    [JsonPropertyName("status")]
    public Status Status { get; set; } = default!;
}

public class Page
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;

    [JsonPropertyName("time_zone")]
    public string TimeZone { get; set; } = default!;

    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; set; }
}

public class Status
{
    [JsonPropertyName("indicator")]
    public string Indicator { get; set; } = default!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = default!;
}
