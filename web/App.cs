using System;
using LeanCloud.Engine;

namespace web {
    public class App {
        // Function
        [LCEngineFunction("Hello")]
        public static string Hello(LCCloudFunctionRequest request) {
            string msg = $"hello, {request.Params["name"]}";
            Console.WriteLine(msg);
            return msg;
        }
    }
}
