using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public class ScopedHub : Hub
    {
        private readonly ScopedConnectionManager _connectionManager;

        public ScopedHub(ScopedConnectionManager connectionManager)
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

        public Task UpdateScopedConnections(string scopeGroupId)
        {
            return _connectionManager.UpdateScopedConnections(this, scopeGroupId);
        }
    }
}