using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.SignalR;
using Common.SignalR.Scoped;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ScopedHub.Api
{
    [Route("api/demo")]
    public class DemoController : ControllerBase
    {
        private readonly HubEventBus _bus;
        private readonly IHubContext<AnyHub> _anyHub;

        public DemoController(HubEventBus bus, IHubContext<AnyHub> anyHub)
        {
            _bus = bus;
            _anyHub = anyHub;
        }


        [Route("UpdateBags")]
        [HttpGet]
        public async Task<string> UpdateBags()
        {
            var myHubContext = _anyHub.AsMyHubContext();
            var onUpdateBagsEvent = new OnUpdateBagsHubContextEvent(myHubContext, "Page-001", "Scope-001", null);
            onUpdateBagsEvent.Bags.Add("Test", "%%%" + DateTime.Now);
            await _bus.Raise(onUpdateBagsEvent);
            return DateTime.Now.ToString("s");
        }

    }
}