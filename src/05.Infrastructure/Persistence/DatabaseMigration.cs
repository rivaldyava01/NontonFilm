﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Infrastructure.Persistence.MySql;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence;

public static class DatabaseMigration
{
    public static async Task ApplyDatabaseMigrationAsync<T>(this IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var logger = serviceProvider.GetRequiredService<ILogger<T>>();
        var persistenceOptions = configuration.GetSection(PersistenceOptions.SectionKey).Get<PersistenceOptions>();

        NontonFilmDbContext context;
        bool isMigrationNeeded;

        switch (persistenceOptions.Provider)
        {
            case PersistenceProvider.None:
                logger.LogWarning($"No {nameof(Persistence)} {CommonDisplayTextFor.Service} is implemented.");
                break;
            case PersistenceProvider.SqlServer:
                context = serviceProvider.GetRequiredService<SqlServerNontonFilmDbContext>();

                isMigrationNeeded = (await context.Database.GetPendingMigrationsAsync()).Any();

                if (isMigrationNeeded)
                {
                    logger.LogInformation("Applying {PersistenceProvider} database migration...", persistenceOptions.Provider);
                    context.Database.Migrate();
                }
                else
                {
                    logger.LogInformation("{PersistenceProvider} database is up to date. No database migration required.", persistenceOptions.Provider);
                }

                break;
            case PersistenceProvider.MySql:
                context = serviceProvider.GetRequiredService<MySqlNontonFilmDbContext>();

                isMigrationNeeded = (await context.Database.GetPendingMigrationsAsync()).Any();

                if (isMigrationNeeded)
                {
                    logger.LogInformation("Applying {PersistenceProvider} database migration...", persistenceOptions.Provider);
                    context.Database.Migrate();
                }
                else
                {
                    logger.LogInformation("{PersistenceProvider} database is up to date. No database migration required.", persistenceOptions.Provider);
                }

                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Persistence)} {nameof(PersistenceOptions.Provider)}: {persistenceOptions.Provider}");
        }
    }
}
