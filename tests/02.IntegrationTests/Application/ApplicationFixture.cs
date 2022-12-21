using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Respawn;
using Respawn.Graph;
using Zeta.NontonFilm.Application;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Authorization;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Application.Services.UserProfile;
using Zeta.NontonFilm.Infrastructure;
using Zeta.NontonFilm.Infrastructure.Logging;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;
using Zeta.NontonFilm.IntegrationTests.Infrastructure.Authorization;
using Zeta.NontonFilm.IntegrationTests.Infrastructure.CurrentUser;
using Zeta.NontonFilm.IntegrationTests.Infrastructure.UserProfile;
using Zeta.NontonFilm.IntegrationTests.Repositories.Users;
using Zeta.NontonFilm.IntegrationTests.Repositories.Users.Models;
using Zeta.NontonFilm.Shared;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.IntegrationTests.Application;

public class ApplicationFixture : IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfigurationRoot _configuration;
    private readonly Respawner _respawner;

    public FakeCurrentUserService CurrentUser { get; private set; }

    public ApplicationFixture()
    {
        SetupEnvironmentVariables();

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{AppContext.BaseDirectory}appsettings.json", false)
            .AddJsonFile($"{AppContext.BaseDirectory}appsettings.{CommonValueFor.EnvironmentName}.json", false);

        _configuration = configurationBuilder.Build();

        var services = new ServiceCollection()
            .AddLogging(logging => logging.AddSimpleConsole(LoggingHelper.SimpleConsoleOptions));

        services.AddShared(_configuration);
        services.AddApplication();
        services.AddInfrastructure(_configuration);

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(webHostEnvironment =>
            webHostEnvironment.EnvironmentName == CommonValueFor.EnvironmentName &&
            webHostEnvironment.ApplicationName == typeof(ApplicationFixture).Assembly.FullName));

        var currentUserServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICurrentUserService))!;
        services.Remove(currentUserServiceDescriptor);

        CurrentUser = new FakeCurrentUserService();
        services.AddTransient(provider => Mock.Of<ICurrentUserService>(currentUser =>
            currentUser.UserId == CurrentUser.UserId &&
            currentUser.Username == CurrentUser.Username &&
            currentUser.PositionId == CurrentUser.PositionId &&
            currentUser.ClientId == CurrentUser.ClientId &&
            currentUser.IpAddress == CurrentUser.IpAddress &&
            currentUser.Geolocation == CurrentUser.Geolocation));

        services.AddTransient<IAuthorizationService, FileBasedAuthorizationService>();
        services.AddTransient<IUserProfileService, FileBasedUserProfileService>();

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        var sqlServerOptions = _configuration.GetSection(SqlServerOptions.SectionKey).Get<SqlServerOptions>();

        EnsureDatabase();

        _respawner = Respawner.CreateAsync(sqlServerOptions.ConnectionString, new RespawnerOptions
        {
            TablesToIgnore = new[] { new Table(TableNameFor.EfMigrationsHistory) }
        }).GetAwaiter().GetResult();
    }

    private static void SetupEnvironmentVariables()
    {
        using var file = File.OpenText("testSettings.json");
        var reader = new JsonTextReader(file);
        var jObject = JObject.Load(reader);

        var variables = jObject
            .GetValue("EnvironmentVariables")!
            .Children<JProperty>();

        foreach (var variable in variables)
        {
            Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
        }
    }

    private void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<SqlServerNontonFilmDbContext>();

        context.Database.Migrate();
    }

    public void RunAsUser(string username, string? positionId = null)
    {
        var user = UserRepository.Users.Where(x => x.Username == username).SingleOrDefault();

        if (user is null)
        {
            throw new NotFoundException(nameof(User), nameof(User.Username), username);
        }

        CurrentUser = new FakeCurrentUserService(user.Id, user.Username, positionId);
    }

    public void RunAsUnauthenticatedUser()
    {
        CurrentUser = new FakeCurrentUserService();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<SqlServerNontonFilmDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task ResetState()
    {
        var sqlServerOptions = _configuration.GetSection(SqlServerOptions.SectionKey).Get<SqlServerOptions>();

        await _respawner.ResetAsync(sqlServerOptions.ConnectionString);

        CurrentUser = new FakeCurrentUserService();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
