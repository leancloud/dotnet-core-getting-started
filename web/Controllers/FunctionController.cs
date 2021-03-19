using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeanCloud.Engine;

namespace web.Controllers {
    [ApiController]
    [Route("{1,1.1}")]
    public class FunctionController : ControllerBase {
        [HttpGet("functions/_ops/metadatas")]
        public object GetFunctions() {
            try {
                return LCEngine.GetFunctions(Request);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("functions/{funcName}")]
        public async Task<object> Run(string funcName, JsonElement body) {
            try {
                return await LCFunctionHandler.HandleRun(funcName, Request, body);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("call/{funcName}")]
        public async Task<object> RPC(string funcName, JsonElement body) {
            try {
                return await LCFunctionHandler.HandleRPC(funcName, Request, body);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
