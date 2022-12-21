using System.Text.Json.Serialization;
using Zeta.NontonFilm.Application;
using Zeta.NontonFilm.Infrastructure;
using Zeta.NontonFilm.Infrastructure.Logging;
using Zeta.NontonFilm.Infrastructure.Persistence;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;
using Zeta.NontonFilm.Shared;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.WebApi.Common.Filters.ApiException;
using Zeta.NontonFilm.WebApi.Common.ModelBindings;
using Zeta.NontonFilm.WebApi.Services;
using Zeta.NontonFilm.WebApi.Services.BackEnd;
using Zeta.NontonFilm.WebApi.Services.Documentation;

Console.WriteLine($"Starting {CommonValueFor.EntryAssemblySimpleName}...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLoggingService();
builder.Services.AddShared(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioning();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ApiExceptionFilterAttribute());
    options.ModelBinderProviders.Insert(0, new SpecialValueModelBinderProvider());
})
.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

using var scope = app.Services.CreateScope();
await scope.ServiceProvider.ApplyDatabaseMigrationAsync<Program>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Running {AssemblyName}", CommonValueFor.EntryAssemblySimpleName);

var initializer = scope.ServiceProvider.GetRequiredService<SqlServerNontonFilmDbContextInitializer>();
await initializer.InitializeAsync();

var sqlServerPersistenceOptions = builder.Configuration.GetSection(SqlServerOptions.SectionKey).Get<SqlServerOptions>();
await initializer.SeedAsync();
if (sqlServerPersistenceOptions.Seeding)
{
    Console.WriteLine("test");

}

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}

app.UseBackEndService(builder.Configuration);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseInfrastructure(builder.Configuration);
app.UseDocumentationService(builder.Configuration);
app.MapControllers();
app.Run();
