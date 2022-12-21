using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Sms;

namespace Zeta.NontonFilm.Infrastructure.Sms.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneSmsService(this IServiceCollection services)
    {
        services.AddSingleton<ISmsService, NoneSmsService>();

        return services;
    }
}
