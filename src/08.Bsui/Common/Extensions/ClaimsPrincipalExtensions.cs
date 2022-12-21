﻿using System.Security.Claims;
using IdentityModel;
using Zeta.NontonFilm.Bsui.Services.Authentication.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Bsui.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(x => x.Type == JwtClaimTypes.Subject)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return new Guid(userId);
    }

    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == JwtClaimTypes.Email)?.Value;
    }

    public static DateTimeOffset? GetAuthenticationTime(this ClaimsPrincipal user)
    {
        var authenticationTime = user.FindFirst(x => x.Type == JwtClaimTypes.AuthenticationTime)?.Value;

        if (string.IsNullOrWhiteSpace(authenticationTime))
        {
            return null;
        }

        return DateTimeOffset.FromUnixTimeSeconds(long.Parse(authenticationTime));
    }

    public static string? GetAccessToken(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == OidcConstants.TokenResponse.AccessToken)?.Value;
    }

    public static string? GetRefreshToken(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == OidcConstants.TokenResponse.RefreshToken)?.Value;
    }

    public static string? GetDisplayName(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == CustomClaimTypes.DisplayName)?.Value;
    }

    public static string? GetEmployeeId(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == CustomClaimTypes.EmployeeId)?.Value;
    }

    public static string? GetCompanyCode(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == CustomClaimTypes.CompanyCode)?.Value;
    }

    public static string? GetPositionId(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == AuthorizationClaimTypes.PositionId)?.Value;
    }

    public static string? GetPositionName(this ClaimsPrincipal user)
    {
        return user.FindFirst(x => x.Type == AuthorizationClaimTypes.PositionName)?.Value;
    }

    public static IEnumerable<string> GetPermissions(this ClaimsPrincipal user)
    {
        return user.Claims.Where(x => x.Type == AuthorizationClaimTypes.Permission).Select(x => x.Value);
    }

    public static bool HasPermission(this ClaimsPrincipal user, string permission)
    {
        return user.Claims.Any(x => x.Type == AuthorizationClaimTypes.Permission && x.Value == permission);
    }

    public static IDictionary<string, string> GetCustomParameters(this ClaimsPrincipal user, bool withoutPrefix = false)
    {
        var claimCustomParameters = user.Claims.Where(x => x.Type.StartsWith(AuthorizationClaimTypes.CustomParameter));

        var customParameters = new Dictionary<string, string>();

        foreach (var claimCustomParameter in claimCustomParameters)
        {
            var claimType = claimCustomParameter.Type;

            if (withoutPrefix)
            {
                claimType = claimType.Replace($"{AuthorizationClaimTypes.CustomParameter}/", string.Empty);
            }

            customParameters.Add(claimType, claimCustomParameter.Value);
        }

        return customParameters;
    }

    public static bool HasCustomParameter(this ClaimsPrincipal user, string key, string value)
    {
        return user.GetCustomParameters().Any(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase) && x.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
    }
}
