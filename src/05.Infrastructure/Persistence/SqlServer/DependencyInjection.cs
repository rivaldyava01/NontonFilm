using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerPersistenceService(this IServiceCollection services, SqlServerOptions sqlServerOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        var migrationsAssembly = typeof(SqlServerNontonFilmDbContext).Assembly.FullName;

        services.AddDbContext<SqlServerNontonFilmDbContext>(options =>
        {
            options.UseSqlServer(sqlServerOptions.ConnectionString, builder =>
            {
                builder.MigrationsAssembly(migrationsAssembly);
                builder.MigrationsHistoryTable(TableNameFor.EfMigrationsHistory, nameof(NontonFilm));
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            options.ConfigureWarnings(wcb => wcb.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
            options.ConfigureWarnings(wcb => wcb.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        });

        services.AddScoped<INontonFilmDbContext>(provider => provider.GetRequiredService<SqlServerNontonFilmDbContext>());
        services.AddScoped<SqlServerNontonFilmDbContextInitializer>();

        healthChecksBuilder.AddSqlServer(
            connectionString: sqlServerOptions.ConnectionString,
            name: $"{nameof(Persistence)} {nameof(PersistenceOptions.Provider)} ({nameof(SqlServer)})");

        return services;
    }
}
