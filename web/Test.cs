using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeanCloud.Engine;
using LeanCloud.Storage;

namespace web {
    public class Test {
        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ReceiversOffline)]
        public static object OnReceiversOffline(LCCloudFunctionRequest request) {
            string alert = request.Params["content"] as string;
            if (alert.Length > 6) {
                alert = alert.Substring(0, 6);
            }
            Dictionary<string, object> pushMessage = new Dictionary<string, object> {
                { "badge", "Increment" },
                { "sound", "default" },
                { "_profile", "dev" },
                { "alert", alert },
            };
            return new Dictionary<string, object> {
                { "pushMessage", JsonSerializer.Serialize(pushMessage) }
            };
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.MessageSent)]
        public static object OnMessageSent(LCCloudFunctionRequest request) {
            Console.WriteLine(JsonSerializer.Serialize(request.Params));
            return default;
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ConversationStart)]
        public static object OnConversationStart(LCCloudFunctionRequest request) {
            List<object> members = request.Params["members"] as List<object>;
            if (members.Count < 4) {
                return new Dictionary<string, object> {
                    { "reject", true },
                    { "code", 1234 },
                    { "detail", "至少邀请 3 人开启对话" }
                };
            }
            return default;
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ConversationStarted)]
        public static object OnConversationStarted(LCCloudFunctionRequest request) {
            string convId = request.Params["convId"] as string;
            Console.WriteLine($"{convId} started");
            return default;
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ConversationAdd)]
        public static object OnConversationAdd(LCCloudFunctionRequest request) {
            if ("Tom".Equals(request.Params["initBy"])) {
                return new Dictionary<string, object> {
                    { "reject", true },
                    { "code", 9890 },
                    { "detail", "会话已封闭，不允许新增成员。" }
                };
            }
            return default;
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ConversationRemove)]
        public static object OnConversationRemove(LCCloudFunctionRequest request) {
            List<string> supporters = new List<string> { "Bast", "Hypnos", "Kthanid" };
            List<string> members = (request.Params["members"] as List<object>)
                .Cast<string>()
                .ToList();
            foreach (object member in members) {
                if (supporters.Contains(member)) {
                    return new Dictionary<string, object> {
                        { "reject", true },
                        { "code", 1928 },
                        { "detail", $"不允许移除官方运营人员 {member}" }
                    };
                }
            }
            return default;
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ConversationAdded)]
        public static async Task OnConversationAdded(LCCloudFunctionRequest request) {
            List<string> members = (request.Params["members"] as List<object>)
                .Cast<string>()
                .ToList();
            if (members.Count > 10) {
                Dictionary<string, object> variables = new Dictionary<string, object> {
                    { "conv_id", request.Params["convId"] }
                };
                try {
                    await LCSMSClient.RequestSMSCode("18200008888", "Group_Notice", "sign_example", variables: variables);
                    Console.WriteLine("Successfully sent text message.");
                } catch (Exception e) {
                    Console.WriteLine($"Failed to send text message. Reason: {e.Message}");
                }
            }
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ConversationRemoved)]
        public static void OnConversationRemoved(LCCloudFunctionRequest request) {
            List<string> members = (request.Params["members"] as List<object>)
                .Cast<string>()
                .ToList();
            string initBy = request.Params["initBy"] as string;
            if (members.Count == 1 && members[0].Equals(initBy)) {
                Console.WriteLine($"{request.Params["convId"]} removed.");
            }
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ConversationUpdate)]
        public static object OnConversationUpdate(LCCloudFunctionRequest request) {
            Dictionary<string, object> attr = request.Params["attr"] as Dictionary<string, object>;
            if (attr != null && attr.ContainsKey("name")) {
                return new Dictionary<string, object> {
                    { "reject", true },
                    { "code", 1949 },
                    { "detail", "对话名称不可修改" }
                };
            }
            return default;
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ClientOnline)]
        public static void OnClientOnline(LCCloudFunctionRequest request) {
            Console.WriteLine($"{request.Params["peerId"]} online.");
        }

        [LCEngineRealtimeHook(LCEngineRealtimeHookType.ClientOffline)]
        public static void OnClientOffline(LCCloudFunctionRequest request) {
            Console.WriteLine($"{request.Params["peerId"]} offline");
        }
    }
}
