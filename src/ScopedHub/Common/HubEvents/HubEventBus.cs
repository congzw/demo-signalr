//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Common;
//using Microsoft.AspNetCore.SignalR;

//namespace ScopedHub.Common
//{
//    public interface IHubEvent
//    {
//        //ScopedConnection Context { get; set; }
//        DateTime RaiseAt { get; }
//        //只有在Hub内触发的事件，才会被赋值
//        Hub RaiseHub { get; set; }
//    }

//    public interface IHubEventHandler
//    {
//        float HandleOrder { set; get; }
//        bool ShouldHandle(IHubEvent hubEvent);
//        Task HandleAsync(IHubEvent hubEvent);
//    }

//    public class HubEventBus
//    {
//        public IEnumerable<IHubEventHandler> HubEventHandlers { get; }

//        public HubEventBus(IEnumerable<IHubEventHandler> hubEventHandlers)
//        {
//            HubEventHandlers = hubEventHandlers;
//        }

//        public async Task Raise(IHubEvent hubEvent)
//        {
//            var hubEventHandlers = ResolveHubEventHandlers()
//                .Where(x => x.ShouldHandle(hubEvent))
//                .OrderBy(x => x.HandleOrder)
//                .ToList();

//            foreach (var hubEventHandler in hubEventHandlers)
//            {
//                await hubEventHandler.HandleAsync(hubEvent).ConfigureAwait(false);
//            }
//        }

//        protected IEnumerable<IHubEventHandler> ResolveHubEventHandlers()
//        {
//            if (HubEventHandlers == null)
//            {
//                return Enumerable.Empty<IHubEventHandler>();
//            }
//            return HubEventHandlers;
//        }
//    }

//    public abstract class BaseHubEvent : IHubEvent
//    {
//        //只有在Hub内触发的事件，raiseHub才会被赋值
//        protected BaseHubEvent(Hub raiseHub = null)
//        {
//            RaiseAt = DateHelper.Instance.GetDateNow();
//            RaiseHub = raiseHub;
//        }
//        public DateTime RaiseAt { get; private set; }
//        public Hub RaiseHub { get; set; }
//    }

//    public class OnConnectedEvent : BaseHubEvent
//    {
//        public OnConnectedEvent(Hub raiseHub) : base(raiseHub)
//        {
//        }
//    }

//    public class OnConnectedEventHandler : IHubEventHandler
//    {
//        private readonly NewScopedConnectionManager _scopedConnectionManager;

//        public OnConnectedEventHandler(NewScopedConnectionManager scopedConnectionManager)
//        {
//            _scopedConnectionManager = scopedConnectionManager;
//        }

//        public float HandleOrder { get; set; }

//        public bool ShouldHandle(IHubEvent hubEvent)
//        {
//            return hubEvent is OnConnectedEvent;
//        }

//        public async Task HandleAsync(IHubEvent hubEvent)
//        {
//            if (!ShouldHandle(hubEvent))
//            {
//                return;
//            }
//            var theEvent = (OnConnectedEvent)hubEvent;
//            await _scopedConnectionManager.OnConnected(theEvent.RaiseHub).ConfigureAwait(false);
//        }
//    }
    
//    public class NewScopedConnectionManager
//    {
//        private readonly IScopedConnectionRepository _repository;

//        public NewScopedConnectionManager(IScopedConnectionRepository repository)
//        {
//            _repository = repository;
//            ScopedContexts = new ConcurrentDictionary<string, HubCallerContext>(StringComparer.OrdinalIgnoreCase);
//        }

//        public IDictionary<string, HubCallerContext> ScopedContexts { get; set; }

//        public HubCallerContext TryGetHubCallerContext(ScopedClientKey scopedClientKey)
//        {
//            var oneKey = scopedClientKey.ToOneKey();
//            ScopedContexts.TryGetValue(oneKey, out var theOne);
//            return theOne;
//        }

//        public async Task OnConnected(OnConnectedEvent theEvent)
//        {
//            var conn = new ScopedConnection();

//            var connectionId = hub.Context.ConnectionId;
//            conn.ConnectionId = connectionId;
//            var now = DateHelper.Instance.GetDateNow();
//            conn.CreateAt = now;
//            conn.LastUpdateAt = now;

//            conn.ScopeGroupId = hub.TryGetQueryParameterValue(nameof(conn.ScopeGroupId), string.Empty);
//            conn.ClientId = hub.TryGetQueryParameterValue(nameof(conn.ClientId), string.Empty);
//            conn.UpdateDesc();

//            _repository.AddOrUpdateScopedConnection(conn);

//            if (!string.IsNullOrWhiteSpace(conn.ClientId))
//            {
//                //scoped clients with same clientId, old should be kicked off
//                var scopedClientKey = ScopedClientKey.Create().WithClientId(conn.ClientId).WithScopeGroupId(conn.ScopeGroupId);
//                var oneKey = scopedClientKey.ToOneKey();
//                if (ScopedContexts.TryGetValue(oneKey, out var oldClientHub))
//                {
//                    await KickSameScopedClient(hub, oldClientHub, scopedClientKey).ConfigureAwait(false);
//                }
//                ScopedContexts[oneKey] = hub.Context;
//            }

//            await hub.Groups.AddToGroupAsync(conn.ConnectionId, conn.ScopeGroupId).ConfigureAwait(false);
//            await UpdateScopedConnections(hub, conn.ScopeGroupId).ConfigureAwait(false);
//        }

//        public async Task OnDisconnected(Hub hub, Exception exception)
//        {
//            var connectionId = hub.Context.ConnectionId;
//            var now = DateHelper.Instance.GetDateNow();

//            var conn = _repository.GetScopedConnection(connectionId);
//            if (conn == null)
//            {
//                //find no conn, should never enter here
//                return;
//            }

//            conn.LastUpdateAt = now;
//            conn.UpdateDesc();

//            if (exception != null)
//            {
//                conn.Desc += ", " + exception.Message;
//            }
//            _repository.RemoveScopedConnection(connectionId);
//            await hub.Groups.RemoveFromGroupAsync(connectionId, conn.ScopeGroupId).ConfigureAwait(false);
//            await UpdateScopedConnections(hub, conn.ScopeGroupId).ConfigureAwait(false);
//        }

//        public Task UpdateScopedConnectionBags(OnUpdateBagsEvent theEvent)
//        {
//            if (theEvent == null || theEvent.Bags == null || theEvent.Bags.Count == 0)
//            {
//                return Task.FromResult(0);
//            }

//            foreach (var bag in theEvent.Bags)
//            {
//                conn.Bags[bag.Key] = bag.Value;
//            }

//            _repository.AddOrUpdateScopedConnection(conn);
//            return UpdateScopedConnections(hub, conn.ScopeGroupId);
//        }

//        public Task UpdateScopedConnections(IHubCallerClients clients, string scopeGroupId)
//        {
//            var scopedConnections = _repository.GetScopedConnections(scopeGroupId);
//            var connections = scopedConnections.OrderBy(x => x.CreateAt).ToList();
//            return clients.Group(scopeGroupId).SendAsync(ScopedConst.ForClient.ScopedConnectionsUpdated(), connections);
//        }

//        public Task UpdateScopedConnections2(string scopeGroupId, IHubContext<Hub> hubContext)
//        {
//            var scopedConnections = _repository.GetScopedConnections(scopeGroupId);
//            var connections = scopedConnections.OrderBy(x => x.CreateAt).ToList();
//            return hubContext.Clients.Group(scopeGroupId).SendAsync(ScopedConst.ForClient.ScopedConnectionsUpdated(), connections);
//        }

//        private async Task KickSameScopedClient(Hub hub, HubCallerContext oldClientHub, ScopedClientKey scopedClientKey)
//        {
//            if (oldClientHub == null)
//            {
//                return;
//            }

//            //already exist, should kick!
//            var oldConnectionId = oldClientHub.ConnectionId;
//            var theConn = _repository.GetScopedConnection(oldConnectionId);
//            if (theConn != null)
//            {
//                var clientProxy = hub.Clients.Client(oldConnectionId);
//                if (clientProxy != null)
//                {
//                    var scopedConnections = _repository.GetScopedConnections(scopedClientKey.ScopeGroupId);
//                    var connections = scopedConnections.OrderBy(x => x.CreateAt).ToList();
//                    var theOne = connections.SingleOrDefault(x => x.ConnectionId.Equals(oldConnectionId));
//                    if (theOne != null)
//                    {
//                        var oneKey = scopedClientKey.ToOneKey();
//                        var message = string.Format("{0} is kicked by another same scoped client!", oneKey);
//                        theOne.Desc = message;
//                    }
//                    await clientProxy.SendAsync(ScopedConst.ForClient.ScopedConnectionsUpdated(), connections).ConfigureAwait(false);
//                }
//            }
//            _repository.RemoveScopedConnection(oldConnectionId);
//            oldClientHub.Abort();
//        }
//    }

//    //public class OnUpdateBagsEvent : BaseHubEvent
//    //{
//    //    public IDictionary<string, object> Bags { get; }

//    //    public OnUpdateBagsEvent(Hub raiseHub, IDictionary<string, object> bags) : base(raiseHub)
//    //    {
//    //        Bags = bags;
//    //    }
//    //}

//    //public class OnUpdateBagsEventHandler : IHubEventHandler
//    //{
//    //    private readonly NewScopedConnectionManager _scopedConnectionManager;

//    //    public OnUpdateBagsEventHandler(NewScopedConnectionManager scopedConnectionManager)
//    //    {
//    //        _scopedConnectionManager = scopedConnectionManager;
//    //        HandleOrder = HubEventHandleOrders.Instance.Forward();
//    //    }

//    //    public float HandleOrder { get; set; }

//    //    public bool ShouldHandle(IHubEvent hubEvent)
//    //    {
//    //        return hubEvent is OnUpdateBagsEvent;
//    //    }

//    //    public async Task HandleAsync(IHubEvent hubEvent)
//    //    {
//    //        if (!ShouldHandle(hubEvent))
//    //        {
//    //            return;
//    //        }
//    //        var theEvent = (OnUpdateBagsEvent)hubEvent;
//    //        await _scopedConnectionManager.UpdateScopedConnectionBags(theEvent.RaiseHub, theEvent.Bags).ConfigureAwait(false);
//    //    }
//    //}
//}
