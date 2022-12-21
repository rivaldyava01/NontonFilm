using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;

namespace Zeta.NontonFilm.Infrastructure.Authentication.ZetaGarde;

public static class DependencyInjection
{
    public static IServiceCollection AddZetaGardeAuthenticationService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<ZetaGardeAuthenticationOptions>(configuration.GetSection(ZetaGardeAuthenticationOptions.SectionKey));

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var zetaGardeAuthenticationOptions = configuration.GetSection(ZetaGardeAuthenticationOptions.SectionKey).Get<ZetaGardeAuthenticationOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = zetaGardeAuthenticationOptions.AuthorityUrl;
                options.Audience = $"{PrefixFor.ApiScope}{zetaGardeAuthenticationOptions.ObjectId}";
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
            name: $"{nameof(Authentication)} {CommonDisplayTextFor.Service} ({nameof(ZetaGarde)})",
            instance: new ZetaGardeAuthenticationHealthCheck(zetaGardeAuthenticationOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
