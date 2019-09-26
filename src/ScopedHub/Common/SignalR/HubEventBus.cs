using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

// ReSharper disable CheckNamespace

namespace Common.SignalR
{
    public interface ISignalREvent
    {
        /// <summary>
        /// 触发事件的时间
        /// </summary>
        DateTime RaiseAt { get; }
    }

    public interface ISignalREventHandler
    {
        float HandleOrder { set; get; }
        bool ShouldHandle(ISignalREvent hubEvent);
    }

    public abstract class BaseHubEvent : ISignalREvent
    {
        protected BaseHubEvent(Hub raiseHub)
        {
            RaiseAt = DateHelper.Instance.GetDateNow();
            RaiseHub = raiseHub;
        }

        public DateTime RaiseAt { get; private set; }
        public Hub RaiseHub { get; private set; }
    }

    public abstract class BaseHubContextEvent : ISignalREvent
    {
        protected BaseHubContextEvent(MyHubContext hubContext)
        {
            RaiseAt = DateHelper.Instance.GetDateNow();
            Context = hubContext;
        }
        public DateTime RaiseAt { get; private set; }
        public MyHubContext Context { get; private set; }
    }

    public interface IHubEventHandler : ISignalREventHandler
    {
        Task HandleAsync(ISignalREvent hubEvent);
    }

    public interface IHubContextEventHandler : ISignalREventHandler
    {
        Task HandleHubContextEventAsync(ISignalREvent hubEvent);
    }

    public class HubEventBus
    {
        public IEnumerable<IHubEventHandler> HubEventHandlers { get; }

        public HubEventBus(IEnumerable<IHubEventHandler> hubEventHandlers)
        {
            HubEventHandlers = hubEventHandlers;
        }

        public async Task Raise(ISignalREvent hubEvent)
        {
            var hubEventHandlers = ResolveHubEventHandlers()
                .Where(x => x.ShouldHandle(hubEvent))
                .OrderBy(x => x.HandleOrder)
                .ToList();

            foreach (var hubEventHandler in hubEventHandlers)
            {
                try
                {
                    if (hubEventHandler is IHubContextEventHandler hubContextEventHandler)
                    {
                        await hubContextEventHandler.HandleHubContextEventAsync(hubEvent).ConfigureAwait(false);
                    }
                    else
                    {
                        await hubEventHandler.HandleAsync(hubEvent).ConfigureAwait(false);
                    }
                }
                catch (Exception e)
                {
                    //todo log
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        protected IEnumerable<IHubEventHandler> ResolveHubEventHandlers()
        {
            if (HubEventHandlers == null)
            {
                return Enumerable.Empty<IHubEventHandler>();
            }
            return HubEventHandlers;
        }
    }

    public class MyHubContext
    {
        public IHubClients Clients { get; set; }
        public IGroupManager Groups { get; set; }
    }

    public static class HubContextExtensions
    {
        public static MyHubContext AsMyHubContext<THub>(this IHubContext<THub> context) where THub : Hub
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var hubContext = new MyHubContext();
            hubContext.Clients = context.Clients;
            hubContext.Groups = context.Groups;
            return hubContext;
        }
    }

    public class HubEventHandleOrders
    {
        public float Forward()
        {
            return -100;
        }

        public float Middle()
        {
            return 0;
        }

        public float Backward()
        {
            return 100;
        }

        public float Between(float one, float two)
        {
            return (one + two) / 2;
        }

        public static HubEventHandleOrders Instance = new HubEventHandleOrders();
    }
}
