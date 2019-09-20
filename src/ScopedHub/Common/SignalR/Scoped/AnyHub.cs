using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    #region demos

    //how to let FooHub scoped:
    //1.  FooHubWrap : FooHub
    //2.  FooHubExtensions.ReplaceFooHub() => AddScopedHub & WrapHub
    //3.  services.ReplaceFooHub()

    #endregion

    public class AnyHub : Hub
    {
        public AnyHub()
        {
        }
    }

    public class AnyHubWrap : AnyHub
    {
        private readonly ScopedConnectionManager _connectionManager;

        public AnyHubWrap(ScopedConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            await _connectionManager.OnConnected(this).ConfigureAwait(false);
            await base.OnConnectedAsync().ConfigureAwait(false);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _connectionManager.OnDisconnected(this, exception).ConfigureAwait(false);
            await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
        }

        public Task UpdateScopedConnectionBags(IDictionary<string, object> bags)
        {
            //online, hide
            return _connectionManager.UpdateScopedConnectionBags(this, bags);
        }

        public Task UpdateScopedConnections(string scopeGroupId)
        {
            return _connectionManager.UpdateScopedConnections(this, scopeGroupId);
        }
    }

    public static class AnyHubExtensions
    {
        public static void ReplaceAnyHub(this IServiceCollection services)
        {
            services.AddScopedHub().WrapHub<AnyHub, AnyHubWrap>();
        }
    }
}