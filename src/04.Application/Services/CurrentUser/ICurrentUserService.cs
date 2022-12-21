using Zeta.NontonFilm.Base.ValueObjects;

namespace Zeta.NontonFilm.Application.Services.CurrentUser;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string Username { get; }
    string ClientId { get; }
    string? PositionId { get; }
    string IpAddress { get; }
    Geolocation? Geolocation { get; }
}
