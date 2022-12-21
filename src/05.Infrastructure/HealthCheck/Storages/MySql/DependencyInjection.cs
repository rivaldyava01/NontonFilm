using Microsoft.Extensions.DependencyInjection;

namespace Zeta.NontonFilm.Infrastructure.HealthCheck.Storages.MySql;

public static class DependencyInjection
{
    public static void AddMySqlStorage(this HealthChecksUIBuilder healthChecksUIBuilder, MySqlOptions mySqlOptions)
    {
        healthChecksUIBuilder.AddMySqlStorage(mySqlOptions.ConnectionString);
    }
}
