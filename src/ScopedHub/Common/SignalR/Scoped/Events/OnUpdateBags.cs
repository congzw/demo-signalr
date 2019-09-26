using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public class OnUpdateBagsEvent : BaseHubEvent
    {
        public IDictionary<string, object> Bags { get; }

        public OnUpdateBagsEvent(Hub raiseHub, IDictionary<string, object> bags) : base(raiseHub)
        {
            Bags = bags;
        }
    }

    public class OnUpdateBagsHubContextEvent : BaseHubContextEvent
    {
        public string ClientId { get; set; }
        public string ScopeGroupId { get; set; }
        public IDictionary<string, object> Bags { get; }

        public OnUpdateBagsHubContextEvent(MyHubContext hubContext, string clientId, string scopeGroupId,
            IDictionary<string, object> bags) : base(hubContext)
        {
            ClientId = clientId;
            ScopeGroupId = scopeGroupId;
            Bags = bags ?? new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class OnUpdateBagsEventHandler : IHubEventHandler
    {
        private readonly ScopedConnectionManager _scopedConnectionManager;

        public OnUpdateBagsEventHandler(ScopedConnectionManager scopedConnectionManager)
        {
            _scopedConnectionManager = scopedConnectionManager;
            HandleOrder = HubEventHandleOrders.Instance.Forward();
        }

        public float HandleOrder { get; set; }

        public bool ShouldHandle(ISignalREvent hubEvent)
        {
            return hubEvent is OnUpdateBagsEvent || hubEvent is OnUpdateBagsHubContextEvent;
        }

        public async Task HandleAsync(ISignalREvent hubEvent)
        {
            if (!ShouldHandle(hubEvent))
            {
                return;
            }

            if (hubEvent is OnUpdateBagsEvent theEvent)
            {
                await _scopedConnectionManager.UpdateScopedConnectionBags(theEvent).ConfigureAwait(false);
            }
            else
            {
                var outsideHubEvent = (OnUpdateBagsHubContextEvent)hubEvent;
                await _scopedConnectionManager.UpdateScopedConnectionBagsOutSideHub(outsideHubEvent).ConfigureAwait(false);
            }
        }
    }
}
