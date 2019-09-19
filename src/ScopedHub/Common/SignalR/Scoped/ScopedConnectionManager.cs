﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public class ScopedConnectionManager
    {
        private readonly IScopedConnectionRepository _repository;

        public ScopedConnectionManager(IScopedConnectionRepository repository)
        {
            _repository = repository;
        }
        
        public async Task OnConnected(Hub hub)
        {
            var conn = new ScopedConnection();
            
            var connectionId = hub.Context.ConnectionId;
            conn.ConnectionId = connectionId;
            var now = DateHelper.Instance.GetDateNow();
            conn.CreateAt = now;
            conn.LastUpdateAt = now;
            
            var paramParse = ScopedConnectionParamParse.Resolve();
            conn.ScopeGroupId = paramParse.TryGetParamValue(hub, nameof(conn.ScopeGroupId));
            conn.ClientId = paramParse.TryGetParamValue(hub, nameof(conn.ClientId));

            conn.UpdateDesc();

            _repository.AddOrUpdateScopedConnection(conn);
            await hub.Groups.AddToGroupAsync(conn.ConnectionId, conn.ScopeGroupId);
            await UpdateScopedConnections(hub, conn.ScopeGroupId);
        }

        public async Task OnDisconnected(Hub hub, Exception exception)
        {
            var connectionId = hub.Context.ConnectionId;
            var now = DateHelper.Instance.GetDateNow();

            var conn = _repository.GetScopedConnection(connectionId);
            if (conn == null)
            {
                //find no conn, should never enter here
                return;
            }

            conn.LastUpdateAt = now;
            conn.UpdateDesc();

            if (exception != null)
            {
                conn.Desc += ", " + exception.Message;
            }
            _repository.RemoveScopedConnection(connectionId);
            await hub.Groups.RemoveFromGroupAsync(connectionId, conn.ScopeGroupId);
            await UpdateScopedConnections(hub, conn.ScopeGroupId);
        }
        
        public Task UpdateScopedConnectionBags(Hub hub, IDictionary<string, object> bags)
        {
            return Task.FromResult(0);
            //todo
        }

        public Task UpdateScopedConnections(Hub hub, string scopeGroupId)
        {
            var scopedConnections = _repository.GetScopedConnections(scopeGroupId);
            var connections = scopedConnections.OrderBy(x => x.CreateAt).ToList();
            return hub.Clients.Group(scopeGroupId).SendAsync(ScopedConnection.UpdateScopedConnectionsCallBackMethod, connections);
        }
    }
}