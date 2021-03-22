using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using LeanCloud;

namespace web {
    public class Program {
        public static void Main(string[] args) {
            LCLogger.LogDelegate = (level, log) => {
                switch (level) {
                    case LCLogLevel.Debug:
                        Console.WriteLine($"[DEBUG] {log}");
                        break;
                    case LCLogLevel.Warn:
                        Console.WriteLine($"[WARN] {log}");
                        break;
                    case LCLogLevel.Error:
                        Console.WriteLine($"[ERROR] {log}");
                        break;
                    default:
                        break;
                }
            };

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
