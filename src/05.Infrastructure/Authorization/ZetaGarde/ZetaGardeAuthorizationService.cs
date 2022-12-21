using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Zeta.NontonFilm.Application.Services.Authorization;
using Zeta.NontonFilm.Infrastructure.Authorization.ZetaGarde.Models.GetAuhtorizationInfo;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Models.GetAuthorizationInfo;
using RestSharp;

namespace Zeta.NontonFilm.Infrastructure.Authorization.ZetaGarde;

public class ZetaGardeAuthorizationService : IAuthorizationService
{
    private readonly ZetaGardeAuthorizationOptions _zetaGardeAuthorizationOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RestClient _restClient;

    public ZetaGardeAuthorizationService(IOptions<ZetaGardeAuthorizationOptions> zetaGardeAuthorizationOptions, IHttpContextAccessor httpContextAccessor)
    {
        _zetaGardeAuthorizationOptions = zetaGardeAuthorizationOptions.Value;
        _httpContextAccessor = httpContextAccessor;
        _restClient = new RestClient(_zetaGardeAuthorizationOptions.BaseUrl);
    }

    public async Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new Exception("HttpContext is null");
        }

        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OidcConstants.TokenResponse.AccessToken);
        var restRequest = new RestRequest($"{_zetaGardeAuthorizationOptions.Endpoints.AuthorizationInfo}/{positionId}?applicationId={_zetaGardeAuthorizationOptions.ObjectId}", Method.Get);

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");

        try
        {
            var restResponse = await _restClient.ExecuteAsync<GetAuhtorizationInfoZetaGarde[]>(restRequest, cancellationToken);

            if (!restResponse.IsSuccessful)
            {
                if (restResponse.ErrorException is not null)
                {
                    throw restResponse.ErrorException;
                }

                throw new Exception($"Failed to retreive {AuthorizationDisplayTextFor.AuthorizationInfo} from {_restClient.BuildUri(restRequest)}");
            }

            if (restResponse.Data is null)
            {
                throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {nameof(GetAuhtorizationInfoZetaGarde)}.");
            }

            var authorizationInfo = new GetAuthorizationInfoResponse();

            if (restResponse.Data.Any())
            {
                var zetaGardeApplication = restResponse.Data.First();

                foreach (var role in zetaGardeApplication.Roles)
                {
                    var authorizationInfoRole = new GetAuthorizationInfoRole
                    {
                        Name = role.Name
                    };

                    if (role.Permissions is not null)
                    {
                        foreach (var permission in role.Permissions)
                        {
                            authorizationInfoRole.Permissions.Add(permission);
                        }
                    }

                    authorizationInfo.Roles.Add(authorizationInfoRole);
                }

                foreach (var customParameter in zetaGardeApplication.CustomParameters.SelectMany(x => x).ToList())
                {
                    authorizationInfo.CustomParameters.Add(new GetAuthorizationInfoCustomParameter
                    {
                        Key = customParameter.Key,
                        Value = customParameter.Value
                    });
                }
            }

            return authorizationInfo;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
