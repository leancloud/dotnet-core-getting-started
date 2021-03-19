using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeanCloud.Engine;

namespace web.Controllers {
    [ApiController]
    [Route("{1,1.1}")]
    public class ClassHookController : ControllerBase {
        [HttpPost("functions/{className}/{hookName}")]
        public async Task<object> Hook(string className, string hookName, JsonElement body) {
            try {
                return await LCClassHookHandler.HandleClassHook(className, hookName, Request, body);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
