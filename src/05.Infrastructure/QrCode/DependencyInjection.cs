using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.QrCode;

namespace Zeta.NontonFilm.Infrastructure.QrCode;

public static class DependencyInjection
{
    public static IServiceCollection AddQrCodeService(this IServiceCollection services)
    {
        services.AddTransient<IQrCodeService, AddQrCodeService>();

        return services;
    }
}
