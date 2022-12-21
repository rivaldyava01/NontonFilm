using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.MySql;

public static class DependencyInjection
{
    public static IServiceCollection AddMySqlPersistenceService(this IServiceCollection services, MySqlOptions mySqlOptions, IHealthChecksBuilder healthChecksBuilder)
    {
        var migrationsAssembly = typeof(MySqlNontonFilmDbContext).Assembly.FullName;

        services.AddDbContext<MySqlNontonFilmDbContext>(options =>
        {
            options.UseMySql(mySqlOptions.ConnectionString, ServerVersion.AutoDetect(mySqlOptions.ConnectionString), builder =>
            {
                builder.MigrationsAssembly(migrationsAssembly);
                builder.MigrationsHistoryTable(TableNameFor.EfMigrationsHistory);
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            options.ConfigureWarnings(wcb => wcb.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
            options.ConfigureWarnings(wcb => wcb.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        });

        services.AddScoped<INontonFilmDbContext>(provider => provider.GetRequiredService<MySqlNontonFilmDbContext>());

        healthChecksBuilder.AddMySql(
            connectionString: mySqlOptions.ConnectionString,
            name: $"{nameof(Persistence)} {nameof(PersistenceOptions.Provider)} ({nameof(MySql)})");

        return services;
    }
}
