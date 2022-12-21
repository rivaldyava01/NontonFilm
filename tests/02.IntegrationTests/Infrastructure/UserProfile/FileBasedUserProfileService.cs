using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.UserProfile;
using Zeta.NontonFilm.Application.Services.UserProfile.Models.GetUserProfile;
using Zeta.NontonFilm.IntegrationTests.Repositories.Users;
using Zeta.NontonFilm.IntegrationTests.Repositories.Users.Models;

namespace Zeta.NontonFilm.IntegrationTests.Infrastructure.UserProfile;

public class FileBasedUserProfileService : IUserProfileService
{
    private readonly ILogger<FileBasedUserProfileService> _logger;

    public FileBasedUserProfileService(ILogger<FileBasedUserProfileService> logger)
    {
        _logger = logger;
    }

    public Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken)
    {
        try
        {
            var user = UserRepository.Users.Where(x => x.Username == username).SingleOrDefault();

            if (user is null)
            {
                throw new NotFoundException(nameof(User), nameof(User.Username), username);
            }

            var result = new GetUserProfileResponse
            {
                Id = user.Id,
                Username = user.Username,
                DisplayName = user.DisplayName,
                EmailAddress = user.Username,
                EmployeeId = user.EmployeeId
            };

            return Task.FromResult(result);

        }

        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error in executing method {nameof(GetUserProfileAsync)}");

            throw;
        }
    }
}
