//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR;
//// ReSharper disable CheckNamespace

//namespace Common.SignalR.Scoped.Events
//{
//    public class OnUpdateBags : BaseHubEvent<IDictionary<string, object>>
//    {
//        //Task UpdateScopedConnectionBags(IDictionary<string, object> bags);
//        public OnUpdateBags(Hub raiseHub, IDictionary<string, object> args) : base(raiseHub, args)
//        {
//        }
//    }

//    public class OnUpdateScopedConnectionBagsHandler : IHubEventHandler
//    {
//        private readonly ScopedConnectionManager _scopedConnectionManager;

//        public OnUpdateScopedConnectionBagsHandler(ScopedConnectionManager scopedConnectionManager)
//        {
//            _scopedConnectionManager = scopedConnectionManager;
//        }

//        public float Order { get; set; }

//        public bool ShouldHandle(IHubEvent hubEvent)
//        {
//            return hubEvent is OnUpdateBags;
//        }

//        public async Task HandleAsync(IHubEvent hubEvent)
//        {
//            if (!ShouldHandle(hubEvent))
//            {
//                return;
//            }
//            var theEvent = (OnUpdateBags)hubEvent;
//            await _scopedConnectionManager.UpdateBags(theEvent.RaiseHub, theEvent.Args).ConfigureAwait(false);
//        }
//    }
//}
