using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Services;
using Zeta.NontonFilm.Bsui.Services.Authentication;
using Zeta.NontonFilm.Bsui.Services.Authorization;
using Zeta.NontonFilm.Bsui.Services.Logging;
using Zeta.NontonFilm.Bsui.Services.Security;
using Zeta.NontonFilm.Client;
using Zeta.NontonFilm.Shared;
using Zeta.NontonFilm.Shared.Common.Constants;

Console.WriteLine($"Starting {CommonValueFor.EntryAssemblySimpleName}...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLoggingService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddShared(builder.Configuration);
builder.Services.AddClient(builder.Configuration);
builder.Services.AddBsui(builder.Configuration);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Running {AssemblyName}", CommonValueFor.EntryAssemblySimpleName);

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}

app.UseBsui(app.Configuration);
app.UseSecurityService(app.Environment);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthenticationService(app.Configuration);
app.UseAuthorizationService(app.Configuration);
app.MapBlazorHub();
app.MapFallbackToPage(CommonRouteFor.Host);
app.Run();
