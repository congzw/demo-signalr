using System;
using Microsoft.AspNetCore.Mvc;

namespace ScopedHub.Api
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Route("getDate")]
        [HttpGet]
        public string GetDate()
        {
            return DateTime.Now.ToString("s");
        }
    }
}
