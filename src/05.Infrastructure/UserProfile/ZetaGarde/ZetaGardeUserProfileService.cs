using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Zeta.NontonFilm.Application.Services.UserProfile;
using Zeta.NontonFilm.Application.Services.UserProfile.Models.GetUserProfile;
using Zeta.NontonFilm.Shared.Common.Constants;
using RestSharp;

namespace Zeta.NontonFilm.Infrastructure.UserProfile.ZetaGarde;

public class ZetaGardeUserProfileService : IUserProfileService
{
    private readonly ZetaGardeUserProfileOptions _zetaGardeUserProfileOptions;
    private readonly RestClient _restClient;

    public ZetaGardeUserProfileService(IOptions<ZetaGardeUserProfileOptions> zetaGardeUserProfileOptions)
    {
        _zetaGardeUserProfileOptions = zetaGardeUserProfileOptions.Value;
        _restClient = new RestClient(_zetaGardeUserProfileOptions.BaseUrl);
    }

    public async Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken)
    {
        var userProfile = new GetUserProfileResponse();
        var accessToken = await GetAccessToken(cancellationToken);
        var restRequest = new RestRequest($"{_zetaGardeUserProfileOptions.Endpoints.Users}/{username}", Method.Get);

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");

        var restResponse = await _restClient.ExecuteAsync(restRequest, cancellationToken);

        if (restResponse.IsSuccessful && !string.IsNullOrWhiteSpace(restResponse.Content))
        {
            var jObjectUser = JObject.Parse(restResponse.Content);

            userProfile.EmailAddress = username;

            var jTokenId = jObjectUser["id"];

            if (jTokenId is not null)
            {
                var jTokenIdValue = jTokenId.Value<string>();

                userProfile.Id = string.IsNullOrWhiteSpace(jTokenIdValue) ? Guid.Empty : new Guid(jTokenIdValue);
            }

            var jTokenEmail = jObjectUser["email"];

            if (jTokenEmail is not null)
            {
                var jTokenEmailValue = jTokenEmail.Value<string>();

                if (!string.IsNullOrWhiteSpace(jTokenEmailValue))
                {
                    userProfile.Username = jTokenEmailValue;
                    userProfile.EmailAddress = jTokenEmailValue;
                }
            }

            var jTokenDisplayName = jObjectUser["displayName"];

            if (jTokenDisplayName is not null)
            {
                var jTokenDisplayNameValue = jTokenDisplayName.Value<string>();
                userProfile.DisplayName = string.IsNullOrWhiteSpace(jTokenDisplayNameValue) ? string.Empty : jTokenDisplayNameValue;
            }

            var jTokenEmployeeId = jObjectUser["employeeId"];

            if (jTokenEmployeeId is not null)
            {
                var jTokenEmployeeIdValue = jTokenEmployeeId.Value<string>();
                userProfile.EmployeeId = string.IsNullOrWhiteSpace(jTokenEmployeeIdValue) ? string.Empty : jTokenEmployeeIdValue;
            }

            var jTokenCompanyCode = jObjectUser["companyCode"];

            if (jTokenCompanyCode is not null)
            {
                var jTokenCompanyCodeValue = jTokenCompanyCode.Value<string>();
                userProfile.CompanyCode = string.IsNullOrWhiteSpace(jTokenCompanyCodeValue) ? string.Empty : jTokenCompanyCodeValue;
            }

            var jTokenMobilePhone = jObjectUser["mobilePhone"];

            if (jTokenMobilePhone is not null)
            {
                var jTokenMobilePhoneValue = jTokenMobilePhone.Value<string>();
                userProfile.MobilePhone = string.IsNullOrWhiteSpace(jTokenMobilePhoneValue) ? string.Empty : jTokenMobilePhoneValue;
            }

            var jTokenExtensionAttributes = jObjectUser["extensionAttributes"];

            if (jTokenExtensionAttributes is not null)
            {
                if (jTokenExtensionAttributes.HasValues)
                {
                    var jTokenGeneral = jTokenExtensionAttributes["General"];

                    if (jTokenGeneral is not null)
                    {
                        if (jTokenGeneral.HasValues)
                        {
                            var jTokenEmailFromExtensionAttributes = jTokenGeneral["Email"];

                            if (jTokenEmailFromExtensionAttributes is not null)
                            {
                                var jTokenEmailValue = jTokenEmailFromExtensionAttributes.Value<string>();

                                if (!string.IsNullOrWhiteSpace(jTokenEmailValue))
                                {
                                    userProfile.EmailAddress = jTokenEmailValue;
                                }
                            }
                        }
                    }
                }
            }
        }

        return userProfile;
    }

    private async Task<string> GetAccessToken(CancellationToken cancellationToken)
    {
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = _zetaGardeUserProfileOptions.TokenUrl,
            ClientId = _zetaGardeUserProfileOptions.ClientId,
            ClientSecret = _zetaGardeUserProfileOptions.ClientSecret,
            Scope = string.Join(' ', _zetaGardeUserProfileOptions.Scopes)
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken);

        if (tokenResponse.IsError)
        {
            throw new InvalidOperationException($"Cannot retreive Access Token from {_zetaGardeUserProfileOptions.TokenUrl}");
        }

        return tokenResponse.AccessToken;
    }
}
