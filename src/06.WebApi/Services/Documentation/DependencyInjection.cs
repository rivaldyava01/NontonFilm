using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.WebApi.Services.Documentation.None;
using Zeta.NontonFilm.WebApi.Services.Documentation.Swagger;

namespace Zeta.NontonFilm.WebApi.Services.Documentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDocumentationService(this IServiceCollection services, IConfiguration configuration)
    {
        var documentationOptions = configuration.GetSection(DocumentationOptions.SectionKey).Get<DocumentationOptions>();

        switch (documentationOptions.Provider)
        {
            case DocumentationProvider.None:
                services.AddNoneDocumentationService();
                break;
            case DocumentationProvider.Swagger:
                services.AddSwaggerDocumentationService(configuration);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Documentation)} {nameof(DocumentationOptions.Provider)}: {documentationOptions.Provider}");
        }

        return services;
    }

    public static IApplicationBuilder UseDocumentationService(this IApplicationBuilder app, IConfiguration configuration)
    {
        var documentationOptions = configuration.GetSection(DocumentationOptions.SectionKey).Get<DocumentationOptions>();

        switch (documentationOptions.Provider)
        {
            case DocumentationProvider.None:
                app.UseNoneDocumentationService();
                break;
            case DocumentationProvider.Swagger:
                app.UseSwaggerDocumentationService(configuration);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Documentation)} {nameof(DocumentationOptions.Provider)}: {documentationOptions.Provider}");
        }

        return app;
    }
}
