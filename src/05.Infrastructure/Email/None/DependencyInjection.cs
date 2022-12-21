using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Email;

namespace Zeta.NontonFilm.Infrastructure.Email.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneEmailService(this IServiceCollection services)
    {
        services.AddSingleton<IEmailService, NoneEmailService>();

        return services;
    }
}
