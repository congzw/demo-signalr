using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public static class ScopedHubExtensions
    {
        public static IServiceCollection AddScopedHub(this IServiceCollection services)
        {
            //services.AddSingleton(sp => new HubEventBus(sp));
            services.AddSingleton<IScopedConnectionRepository, MemoryScopedConnectionRepository>();
            services.AddSingleton<ScopedConnectionManager>();
            return services;
        }

        public static void WrapHub<THub, THubWrap>(this IServiceCollection services)
            where THub : Hub
            where THubWrap : THub
        {
            services.AddScoped<THubWrap>();
            services.Replace(ServiceDescriptor.Scoped<THub>(sp =>
            {
                var anyHubWrap = sp.GetService<THubWrap>();
                return anyHubWrap;
            }));
        }
    }
}
