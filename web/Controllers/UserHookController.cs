using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeanCloud.Engine;

namespace web.Controllers {
    [ApiController]
    [Route("{1,1.1}/functions")]
    public class UserHookController : ControllerBase {
        [HttpPost("onVerified/sms")]
        public async Task<object> HookSMSVerification(JsonElement body) {
            try {
                return await LCUserHookHandler.HandleVerifiedSMS(Request, body);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("onVerified/email")]
        public async Task<object> HookEmailVerification(JsonElement body) {
            try {
                return await LCUserHookHandler.HandleVerifiedEmail(Request, body);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("_User/onLogin")]
        public async Task<object> HookLogin(JsonElement body) {
            try {
                return await LCUserHookHandler.HandleLogin(Request, body);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
