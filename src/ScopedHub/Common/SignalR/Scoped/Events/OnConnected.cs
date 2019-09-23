//// ReSharper disable CheckNamespace

//using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR;

//namespace Common.SignalR.Scoped.Events
//{
//    public class OnConnectedEvent : BaseHubEvent
//    {
//        public OnConnectedEvent(Hub raiseHub) : base(raiseHub)
//        {
//        }
//    }

//    public class OnConnectedEventHandler : IHubEventHandler
//    {
//        private readonly ScopedConnectionManager _scopedConnectionManager;

//        public OnConnectedEventHandler(ScopedConnectionManager scopedConnectionManager)
//        {
//            _scopedConnectionManager = scopedConnectionManager;
//        }

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

//        public float Order { get; set; }
//    }
//}
