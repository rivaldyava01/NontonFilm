﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Zeta.NontonFilm.Infrastructure.HealthCheck.Storages.SqlServer;

public static class DependencyInjection
{
    public static void AddSqlServerStorage(this HealthChecksUIBuilder healthChecksUIBuilder, SqlServerOptions sqlServerOptions)
    {
        healthChecksUIBuilder.AddSqlServerStorage(sqlServerOptions.ConnectionString, options =>
        {
            options.ConfigureWarnings(wcb => wcb.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
            options.ConfigureWarnings(wcb => wcb.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
        });
    }
}
