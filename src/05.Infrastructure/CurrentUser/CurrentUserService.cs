using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Base.ValueObjects;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Infrastructure.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }

            var subject = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);

            if (string.IsNullOrWhiteSpace(subject))
            {
                return null;
            }

            return new Guid(subject);
        }
    }

    public string Username
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return DefaultTextFor.SystemBackgroundJob;
            }

            return _httpContextAccessor.HttpContext.User.FindFirstValue(JwtClaimTypes.Email) ?? DefaultTextFor.Unknown;
        }
    }

    public string? PositionId => _httpContextAccessor.HttpContext?.Request.Headers[HttpHeaderName.ZtcbPositionId].FirstOrDefault();

    public string ClientId
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return DefaultTextFor.SystemBackgroundJob;
            }

            return _httpContextAccessor.HttpContext.User.FindFirstValue(JwtClaimTypes.ClientId) ?? DefaultTextFor.Unknown;
        }
    }

    public string IpAddress
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return DefaultTextFor.SystemBackgroundJob;
            }

            var ipAddress = _httpContextAccessor.HttpContext.Request.Headers[HttpHeaderName.ZtcbIpAddress].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                return ipAddress;
            }

            var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

            if (remoteIpAddress is not null)
            {
                return remoteIpAddress.ToString();
            }

            return DefaultTextFor.Unknown;
        }
    }

    public Geolocation? Geolocation
    {
        get
        {
            var geolocationText = _httpContextAccessor.HttpContext?.Request.Headers[HttpHeaderName.ZtcbGeolocation].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(geolocationText))
            {
                return null;
            }

            return Geolocation.From(geolocationText);
        }
    }

    public IList<string> Permissions
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return new List<string>();
            }

            return _httpContextAccessor.HttpContext.User.FindAll(AuthorizationClaimTypes.Permission).Select(x => x.Value).ToList();
        }
    }
}
