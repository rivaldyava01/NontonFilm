using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zeta.NontonFilm.Shared.Audits.Options;

public static class DependencyInjection
{
    public static IServiceCollection AddAuditOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuditOptions>(configuration.GetSection(AuditOptions.SectionKey));

        return services;
    }
}
