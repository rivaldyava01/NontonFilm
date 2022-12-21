using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;

namespace Zeta.NontonFilm.Infrastructure.Authentication.IS4IM;

public static class DependencyInjection
{
    public static IServiceCollection AddIS4IMAuthenticationService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<IS4IMAuthenticationOptions>(configuration.GetSection(IS4IMAuthenticationOptions.SectionKey));

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var is4imAuthenticationOptions = configuration.GetSection(IS4IMAuthenticationOptions.SectionKey).Get<IS4IMAuthenticationOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = is4imAuthenticationOptions.AuthorityUrl;
                options.Audience = $"{PrefixFor.ApiScope}{is4imAuthenticationOptions.ObjectId}";
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Authentication)} Service ({nameof(IS4IM)})",
            instance: new IS4IMAuthenticationHealthCheck(is4imAuthenticationOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
