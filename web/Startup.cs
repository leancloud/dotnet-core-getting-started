using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace web {
    public class Startup {
        private readonly string[] LCEngineCORSMethods = new string[] {
            "PUT",
            "GET",
            "POST",
            "DELETE",
            "OPTIONS"
        };
        private readonly string[] LCEngineCORSHeaders = new string[] {
            "Content-Type",
            "X-AVOSCloud-Application-Id",
            "X-AVOSCloud-Application-Key",
            "X-AVOSCloud-Application-Production",
            "X-AVOSCloud-Client-Version",
            "X-AVOSCloud-Request-Sign",
            "X-AVOSCloud-Session-Token",
            "X-AVOSCloud-Super-Key",
            "X-LC-Hook-Key",
            "X-LC-Id",
            "X-LC-Key",
            "X-LC-Prod",
            "X-LC-Session",
            "X-LC-Sign",
            "X-LC-UA",
            "X-Requested-With",
            "X-Uluru-Application-Id",
            "X-Uluru-Application-Key",
            "X-Uluru-Application-Production",
            "X-Uluru-Client-Version",
            "X-Uluru-Session-Token"
        };

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder.AllowAnyOrigin()
                        .WithMethods(LCEngineCORSMethods)
                        .WithHeaders(LCEngineCORSHeaders);
                });
            });
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
            app.UseStaticFiles();

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
