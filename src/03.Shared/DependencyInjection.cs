using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Shared.Audits.Options;

namespace Zeta.NontonFilm.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        #region Essential Options
        services.AddAuditOptions(configuration);
        #endregion Essential Options

        #region Business Options
        #endregion Business Options

        return services;
    }
}