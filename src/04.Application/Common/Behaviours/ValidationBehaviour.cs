using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUser;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUser, IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _currentUser = currentUser;
        _validators = validators;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(failure => failure is not null).ToList();

            if (failures.Any())
            {
                var requestName = typeof(TRequest).Name;
                var formattedRequest = request.ToPrettyJson();
                var username = _currentUser.Username;
                var ipAddress = _currentUser.IpAddress;
                var latitude = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Latitude.ToString();
                var longitude = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Longitude.ToString();
                var accuracy = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Accuracy.ToString();
                var exception = new ApplicationValidationException(failures);

                _logger.LogError(exception, "Validation failed for {RequestName} by {Username} from {IpAddress} at {Latitude}||{Longitude}||{Accuracy}.\n{RequestName}\n{RequestObject}",
                   requestName, username, ipAddress, latitude, longitude, accuracy, requestName, formattedRequest);

                throw exception;
            }
        }
    }
}
