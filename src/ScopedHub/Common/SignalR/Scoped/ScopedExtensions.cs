using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public static class ScopedExtensions
    {
        public static IServiceCollection AddScopedHub(this IServiceCollection services)
        {
            services.AddScoped<HubEventBus>();
            services.AddAllImpl<IHubEventHandler>(ServiceLifetime.Scoped);

            services.AddSingleton<IScopedConnectionRepository, MemoryScopedConnectionRepository>();
            services.AddSingleton<ScopedConnectionManager>();
            return services;
        }

        public static ScopedConnection TryParseFromHubCallerContext(this HubCallerContext callerContext, params string[] queryParams)
        {
            var context = new ScopedConnection();
            context.ConnectionId = callerContext.ConnectionId;
            context.ScopeGroupId = callerContext.TryGetQueryParameterValue(nameof(context.ScopeGroupId), string.Empty);
            context.ClientId = callerContext.TryGetQueryParameterValue(nameof(context.ClientId), string.Empty);
            foreach (var queryParam in queryParams)
            {
                var queryValue = callerContext.TryGetQueryParameterValue<object>(queryParam, null);
                context.SetBagValue(queryParam, queryValue);
            }
            return context;
        }
    }
}
