using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Zeta.NontonFilm.Bsui.Services.Authentication.Constants;
using Zeta.NontonFilm.Bsui.Services.Authentication.Extensions;
using Zeta.NontonFilm.Bsui.Services.FrontEnd;

namespace Zeta.NontonFilm.Bsui.Services.Authentication.ZetaGarde;

public static class DependencyInjection
{
    public static IServiceCollection AddZetaGardeAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ZetaGardeAuthenticationOptions>(configuration.GetSection(ZetaGardeAuthenticationOptions.SectionKey));

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var zetaGardeAuthenticationOptions = configuration.GetSection(ZetaGardeAuthenticationOptions.SectionKey).Get<ZetaGardeAuthenticationOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = zetaGardeAuthenticationOptions.AuthorityUrl;
                options.ClientId = zetaGardeAuthenticationOptions.ClientId;
                options.ClientSecret = zetaGardeAuthenticationOptions.ClientSecret;
                options.ResponseType = OidcConstants.ResponseTypes.CodeIdToken;
                options.SaveTokens = true;
                options.UseTokenLifetime = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add(ZetaGardeScopes.ApiAuth);
                options.Scope.Add(ZetaGardeScopes.UserRole);
                options.Scope.Add(ZetaGardeScopes.UserRead);
                options.Scope.Add(ZetaGardeScopes.ApplicationRead);
                options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                options.Scope.Add(zetaGardeAuthenticationOptions.ApiAudienceScope);

                options.ClaimActions.MapJsonKey(ClaimTypes.Name, JwtClaimTypes.Email, ClaimValueTypes.String);
                options.ClaimActions.MapUniqueJsonKey(CustomClaimTypes.CompanyCode, CustomClaimTypes.CompanyCode, ClaimValueTypes.String);
                options.ClaimActions.MapUniqueJsonKey(CustomClaimTypes.EmployeeId, CustomClaimTypes.EmployeeId, ClaimValueTypes.String);
                options.ClaimActions.MapUniqueJsonKey(CustomClaimTypes.DisplayName, CustomClaimTypes.DisplayName, ClaimValueTypes.String);

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenResponseReceived = async context => await context.SaveTokens(),
                    OnUserInformationReceived = async context => await context.LoadAuthorizationClaims(),
                    OnRedirectToIdentityProvider = context =>
                    {
                        if (zetaGardeAuthenticationOptions.Redirect.Enabled)
                        {
                            var frontEndOptions = configuration.GetSection(FrontEndOptions.SectionKey).Get<FrontEndOptions>();
                            var httpRequest = context.HttpContext.Request;
                            var redirectUri = $"{httpRequest.Scheme}://{httpRequest.Host}{frontEndOptions.BasePath}/signin-oidc";

                            context.ProtocolMessage.RedirectUri = zetaGardeAuthenticationOptions.Redirect.Url;
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        return services;
    }

    public static IApplicationBuilder UseZetaGardeAuthentication(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseAuthentication();

        var zetaGardeAuthenticationOptions = configuration.GetSection(ZetaGardeAuthenticationOptions.SectionKey).Get<ZetaGardeAuthenticationOptions>();

        var proxyIpAddresses = new List<IPAddress>();

        if (zetaGardeAuthenticationOptions.Proxy.Enabled)
        {
            foreach (var host in zetaGardeAuthenticationOptions.Proxy.Hosts)
            {
                proxyIpAddresses.Add(IPAddress.Parse(host));
            }

            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";

                return next();
            });
        }

        var forwardedHeadersOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };

        forwardedHeadersOptions.KnownProxies.Clear();

        foreach (var proxyIpAddress in proxyIpAddresses)
        {
            forwardedHeadersOptions.KnownProxies.Add(proxyIpAddress);
        }

        app.UseForwardedHeaders(forwardedHeadersOptions);

        return app;
    }
}
