using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde.Models.GetAuhtorizationInfo;
using Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde.Models.GetPositions;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Models.GetAuthorizationInfo;
using Zeta.NontonFilm.Shared.Services.Authorization.Models.GetPositions;
using RestSharp;

namespace Zeta.NontonFilm.Bsui.Services.Authorization.ZetaGarde;

public class ZetaGardeAuthorizationService : IAuthorizationService
{
    private readonly ZetaGardeAuthorizationOptions _zetaGardeAuthorizationOptions;
    private readonly RestClient _restClient;

    public ZetaGardeAuthorizationService(IOptions<ZetaGardeAuthorizationOptions> zetaGardeAuthorizationOptions)
    {
        _zetaGardeAuthorizationOptions = zetaGardeAuthorizationOptions.Value;
        _restClient = new RestClient(_zetaGardeAuthorizationOptions.BaseUrl);
    }

    public async Task<GetPositionsResponse> GetPositionsAsync(string username, string accessToken, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"{_zetaGardeAuthorizationOptions.Endpoints.Positions}/{username}", Method.Get);

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");

        try
        {
            var restResponse = await _restClient.ExecuteAsync<GetPositionsZetaGardePersona[]>(restRequest, cancellationToken);

            if (!restResponse.IsSuccessful)
            {
                if (restResponse.ErrorException is null)
                {
                    throw new Exception($"Failed to retreive {AuthorizationDisplayTextFor.Positions} from {_restClient.BuildUri(restRequest)}");
                }

                throw restResponse.ErrorException;
            }

            if (restResponse.Data is null)
            {
                throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {nameof(GetPositionsZetaGardePersona)}.");
            }

            var getPositionsResponse = new GetPositionsResponse();

            foreach (var persona in restResponse.Data)
            {
                foreach (var position in persona.Positions)
                {
                    if (!getPositionsResponse.Positions.Any(x => x.Id == position.Id))
                    {
                        getPositionsResponse.Positions.Add(new GetPositionsPosition
                        {
                            Id = position.Id,
                            Name = position.Name,
                            PersonaType = persona.Type
                        });
                    }
                }
            }

            return getPositionsResponse;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, string accessToken, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"{_zetaGardeAuthorizationOptions.Endpoints.AuthorizationInfo}/{positionId}?applicationId={_zetaGardeAuthorizationOptions.ObjectId}", Method.Get);

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");

        try
        {
            var restResponse = await _restClient.ExecuteAsync<GetAuhtorizationInfoZetaGarde[]>(restRequest, cancellationToken);

            if (!restResponse.IsSuccessful)
            {
                if (restResponse.ErrorException is null)
                {
                    throw new Exception($"Failed to retreive {AuthorizationDisplayTextFor.AuthorizationInfo} from {_restClient.BuildUri(restRequest)}");
                }

                throw restResponse.ErrorException;
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
