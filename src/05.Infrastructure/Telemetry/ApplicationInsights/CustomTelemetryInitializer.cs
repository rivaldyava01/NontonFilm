﻿using System.Security.Claims;
using IdentityModel;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Zeta.NontonFilm.Base.ValueObjects;
using Zeta.NontonFilm.Infrastructure.AppInfo;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Telemetry.ApplicationInsights;

public class CustomTelemetryInitializer : ITelemetryInitializer
{
    private readonly AppInfoOptions _appInfoOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomTelemetryInitializer(IOptions<AppInfoOptions> appInfoOptions, IHttpContextAccessor httpContextAccessor)
    {
        _appInfoOptions = appInfoOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Component.Version = CommonValueFor.EntryAssemblyInformationalVersion;

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            if (httpContext.User is not null)
            {
                telemetry.Context.User.AuthenticatedUserId = httpContext.User.FindFirstValue(JwtClaimTypes.Email) ?? DefaultTextFor.Unknown;
                telemetry.Context.User.Id = httpContext.User.FindFirstValue(JwtClaimTypes.Subject) ?? Guid.Empty.ToString();
            }

            var clientIp = httpContext.Request.Headers[HttpHeaderName.ZtcbIpAddress].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(clientIp))
            {
                var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

                clientIp = remoteIpAddress is not null ? remoteIpAddress.ToString() : DefaultTextFor.SystemBackgroundJob;
            }

            telemetry.Context.Location.Ip = clientIp;

            if (_httpContextAccessor.HttpContext is not null)
            {
                var geolocationText = _httpContextAccessor.HttpContext.Request.Headers[HttpHeaderName.ZtcbGeolocation].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(geolocationText))
                {
                    var geolocation = Geolocation.From(geolocationText);

                    telemetry.Context.GlobalProperties[nameof(Geolocation.Latitude)] = geolocation.Latitude.ToString();
                    telemetry.Context.GlobalProperties[nameof(Geolocation.Longitude)] = geolocation.Longitude.ToString();
                    telemetry.Context.GlobalProperties[nameof(Geolocation.Accuracy)] = geolocation.Accuracy.ToString();
                }
            }
        }

        if (string.IsNullOrWhiteSpace(telemetry.Context.Cloud.RoleName))
        {
            var suffix = CommonValueFor.EnvironmentName == EnvironmentNames.Production ? string.Empty : $" {CommonValueFor.EnvironmentName}";

            telemetry.Context.Cloud.RoleName = $"{_appInfoOptions.FullName}{suffix}";
        }
    }
}
