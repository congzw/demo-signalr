//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR;
//// ReSharper disable CheckNamespace

//namespace Common.SignalR.Scoped.Events
//{
//    public class OnDisconnectedEvent : BaseHubEvent<Exception>
//    {
//        public OnDisconnectedEvent(Hub raiseHub, Exception args) : base(raiseHub, args)
//        {
//        }
//    }

//    public class OnDisconnectedEventHandler : IHubEventHandler
//    {
//        private readonly ScopedConnectionManager _scopedConnectionManager;

//        public OnDisconnectedEventHandler(ScopedConnectionManager scopedConnectionManager)
//        {
//            _scopedConnectionManager = scopedConnectionManager;
//        }

//        public float Order { get; set; }

//        public bool ShouldHandle(IHubEvent hubEvent)
//        {
//            return hubEvent is OnDisconnectedEvent;
//        }

//        public async Task HandleAsync(IHubEvent hubEvent)
//        {
//            if (!ShouldHandle(hubEvent))
//            {
//                return;
//            }
//            var theEvent = (OnDisconnectedEvent)hubEvent;
//            await _scopedConnectionManager.OnDisconnected(theEvent.RaiseHub, theEvent.Args).ConfigureAwait(false);
//        }
//    }
//}
