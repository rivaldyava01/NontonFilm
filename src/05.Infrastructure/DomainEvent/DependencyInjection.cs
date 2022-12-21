using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.DomainEvent;

namespace Zeta.NontonFilm.Infrastructure.DomainEvent;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainEventService(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventService, DomainEventService>();

        return services;
    }
}
