using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Base.ValueObjects;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.IntegrationTests.Infrastructure.CurrentUser;

public class FakeCurrentUserService : ICurrentUserService
{
    public Guid? UserId { get; }
    public string Username { get; }
    public string ClientId => DefaultTextFor.SystemBackgroundJob;
    public string? PositionId { get; }
    public string IpAddress => DefaultTextFor.SystemBackgroundJob;
    public Geolocation? Geolocation { get; }

    public FakeCurrentUserService()
    {
        Username = DefaultTextFor.SystemBackgroundJob;
    }

    public FakeCurrentUserService(Guid userId, string username, string? positionId)
    {
        UserId = userId;
        Username = username;
        PositionId = positionId;
    }
}
