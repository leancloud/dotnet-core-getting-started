using System.Diagnostics;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web.Models;

namespace test {
    //[EnableCors("LCEngineCORS")]
    [ApiController]
    [Route("test")]
    public class TestController : Controller {
        [HttpGet]
        public object Test() {
            return "test";
        }
    }
}
