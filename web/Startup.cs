using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using LeanCloud;
using LeanCloud.Engine;

namespace web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddLogging(builder => {
                builder
                    .AddFilter("Microsoft", LogLevel.Error)
                    .AddFilter("System", LogLevel.Error)
                    .AddConsole();
            });

            LCLogger.LogDelegate = (level, log) => {
                switch (level) {
                    //case LCLogLevel.Debug:
                    //    Console.WriteLine($"[DEBUG] {log}");
                    //    break;
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
            LCEngine.Initialize(services);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            bool IsDevelopment = Environment.GetEnvironmentVariable("LEANCLOUD_APP_ENV") == "development";
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = IsDevelopment ?
                    new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot")) :
                    new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "release", "wwwroot"))
            });

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
