using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public static class ScopedHubExtensions
    {
        public static IServiceCollection AddScopedHub(this IServiceCollection services)
        {
            services.AddScoped<HubEventBus>();
            services.AddAllImpl<IHubEventHandler>(ServiceLifetime.Scoped);

            services.AddSingleton<IScopedConnectionRepository, MemoryScopedConnectionRepository>();
            services.AddSingleton<ScopedConnectionManager>();
            return services;
        }
    }
}
