using Microsoft.AspNetCore.Mvc;
using LeanCloud.Engine;

namespace web.Controllers {
    [ApiController]
    [Route("__engine/{1,1.1}")]
    public class PingController : ControllerBase {
        [HttpGet("ping")]
        public object Get() {
            return LCPingHandler.HandlePing();
        }
    }
}
