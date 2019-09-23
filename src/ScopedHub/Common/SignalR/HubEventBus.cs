//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.Extensions.DependencyInjection;

//// ReSharper disable CheckNamespace

//namespace Common.SignalR
//{
//    public interface IHubEvent
//    {
//        DateTime DateTimeEventOccurred { get; }

//        Hub RaiseHub { get; }
//    }
//    public abstract class BaseHubEvent : IHubEvent
//    {
//        public DateTime DateTimeEventOccurred { get; private set; }

//        public Hub RaiseHub { get; private set; }

//        protected BaseHubEvent(Hub raiseHub)
//        {
//            DateTimeEventOccurred = DateTime.Now;
//            RaiseHub = raiseHub;
//        }
//    }

//    public abstract class BaseHubEvent<T> : BaseHubEvent
//    {
//        public T Args { get; set; }

//        protected BaseHubEvent(Hub raiseHub, T args) : base(raiseHub)
//        {
//            Args = args;
//        }
//    }

//    public interface IHubEventHandler
//    {
//        float Order { set; get; }
//        bool ShouldHandle(IHubEvent hubEvent);
//        Task HandleAsync(IHubEvent hubEvent);
//    }

//    public class HubEventBus
//    {
//        public HubEventBus(IServiceProvider sp)
//        {
//            ServiceProvider = sp;
//        }

//        public IServiceProvider ServiceProvider { get; set; }

//        public async Task Raise(IHubEvent hubEvent)
//        {
//            var hubEventHandlers = ResolveHubEventHandlers()
//                .Where(x => x.ShouldHandle(hubEvent))
//                .OrderBy(x => x.Order)
//                .ToList();

//            foreach (var hubEventHandler in hubEventHandlers)
//            {
//                await hubEventHandler.HandleAsync(hubEvent).ConfigureAwait(false);
//            }
//        }

//        protected IEnumerable<IHubEventHandler> ResolveHubEventHandlers()
//        {
//            var eventHandlers = ServiceProvider.GetServices<IHubEventHandler>();
//            if (eventHandlers == null)
//            {
//                return Enumerable.Empty<IHubEventHandler>();
//            }
//            return eventHandlers;
//        }
//    }
//}
