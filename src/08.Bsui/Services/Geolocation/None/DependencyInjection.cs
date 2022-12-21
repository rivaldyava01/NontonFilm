﻿using Darnton.Blazor.DeviceInterop.Geolocation;

namespace Zeta.NontonFilm.Bsui.Services.Geolocation.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneGeolocationService(this IServiceCollection services)
    {
        services.AddScoped<IGeolocationService, NoneGeolocationService>();

        return services;
    }
}
