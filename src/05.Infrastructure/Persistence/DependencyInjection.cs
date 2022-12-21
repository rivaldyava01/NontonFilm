using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Infrastructure.Persistence.MySql;
using Zeta.NontonFilm.Infrastructure.Persistence.None;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        var persistenceOptions = configuration.GetSection(PersistenceOptions.SectionKey).Get<PersistenceOptions>();

        switch (persistenceOptions.Provider)
        {
            case PersistenceProvider.None:
                services.AddNonePersistenceService();
                break;
            case PersistenceProvider.SqlServer:
                var sqlServerOptions = configuration.GetSection(SqlServerOptions.SectionKey).Get<SqlServerOptions>();
                services.AddSqlServerPersistenceService(sqlServerOptions, healthChecksBuilder);
                break;
            case PersistenceProvider.MySql:
                var mySqlOptions = configuration.GetSection(MySqlOptions.SectionKey).Get<MySqlOptions>();
                services.AddMySqlPersistenceService(mySqlOptions, healthChecksBuilder);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Persistence)} {nameof(PersistenceOptions.Provider)}: {persistenceOptions.Provider}");
        }

        return services;
    }
}
