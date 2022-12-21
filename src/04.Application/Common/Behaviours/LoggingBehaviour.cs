using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUser;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var formattedRequest = request.ToPrettyJson();
        var username = _currentUser.Username;
        var ipAddress = _currentUser.IpAddress;
        var latitude = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Latitude.ToString();
        var longitude = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Longitude.ToString();
        var accuracy = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Accuracy.ToString();

        _logger.LogInformation("{RequestName} by {Username} from {IpAddress} at {Latitude}||{Longitude}||{Accuracy}.\n{RequestName}\n{RequestObject}",
           requestName, username, ipAddress, latitude, longitude, accuracy, requestName, formattedRequest);

        return Task.CompletedTask;
    }
}
