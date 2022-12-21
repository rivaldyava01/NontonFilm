﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.Options;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Authentication;
using Zeta.NontonFilm.Application.Services.Authorization;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Shared.Services.Authentication.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private const string StartingErrorMessage = "The server is refusing to process the request because the user";
    private readonly bool _usingAuthentication;
    private readonly bool _usingAuthorization;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationBehaviour(IOptions<AuthenticationOptions> authenticationOptions, IOptions<AuthorizationOptions> authorizationOptions, ICurrentUserService currentUserService, IAuthorizationService authorizationService)
    {
        _usingAuthentication = authenticationOptions.Value.Provider is not AuthenticationProvider.None;
        _usingAuthorization = authorizationOptions.Value.Provider is not AuthenticationProvider.None;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (!authorizeAttributes.Any())
        {
            return await next();
        }

        if (!_usingAuthentication)
        {
            return await next();
        }

        if (!_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException($"{StartingErrorMessage} is not authenticated.");
        }

        if (!_usingAuthorization)
        {
            return await next();
        }

        var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

        if (!authorizeAttributesWithPolicies.Any())
        {
            return await next();
        }

        if (string.IsNullOrWhiteSpace(_currentUserService.PositionId))
        {
            throw new ForbiddenAccessException($"{StartingErrorMessage} {_currentUserService.Username} does not have {AuthenticationDisplayTextFor.PositionId}.");
        }

        var authorizationInfo = await _authorizationService.GetAuthorizationInfoAsync(_currentUserService.PositionId, cancellationToken);

        foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
        {
            var authorized = authorizationInfo.Roles.SelectMany(x => x.Permissions).Any(x => x.Equals(policy, StringComparison.OrdinalIgnoreCase));

            if (!authorized)
            {
                throw new ForbiddenAccessException($"{StartingErrorMessage} {_currentUserService.Username} with {AuthenticationDisplayTextFor.PositionId} {_currentUserService.PositionId} does not have the following {AuthorizationClaimTypes.Permission}: {policy}");
            }
        }

        return await next();
    }
}
