using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public class ScopedHub : Hub
    {
        private readonly ScopedHubManager _connectionManager;

        public ScopedHub(ScopedHubManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            await _connectionManager.OnConnected(this);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _connectionManager.OnDisconnected(this, exception);
            await base.OnDisconnectedAsync(exception);
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
}