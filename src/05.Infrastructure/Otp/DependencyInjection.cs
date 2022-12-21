﻿using Microsoft.Extensions.DependencyInjection;
using Zeta.NontonFilm.Application.Services.Otp;

namespace Zeta.NontonFilm.Infrastructure.Otp;

public static class DependencyInjection
{
    public static IServiceCollection AddOtpService(this IServiceCollection services)
    {
        services.AddTransient<IOtpService, OtpService>();

        return services;
    }
}
