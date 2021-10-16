/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Headtrip.Services
{
    public static class DependencyService
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // services.AddSingleton<ISqliteService, Platforms.Android.Dependencies.AndroidSqliteService>();
            return services;
        }

        public static IServiceCollection ConfigureViewModels(this IServiceCollection services)
        {
            services.AddTransient<MonitorViewModel>();
            services.AddTransient<GraphViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<HelpViewModel>();
            return services;
        }
    }
}