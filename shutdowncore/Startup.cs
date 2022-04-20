using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace shutdowncore
{
    public class Startup
    {
        private IWebHostEnvironment env;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            this.env = env;

            lifetime.ApplicationStarted.Register(OnAppStarted);
            lifetime.ApplicationStopping.Register(OnAppStopping);
            lifetime.ApplicationStopped.Register(OnAppStopped);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            void OnAppStarted()
            {
                string path = $"{env.WebRootPath}/AppLog.txt";
                Console.WriteLine(path);
                string contents = $"App started at {DateTime.Now}";
                File.AppendAllText(path, contents);
            }

            void OnAppStopping()
            {
                string path = $"{env.WebRootPath}/AppLog.txt";
                Console.WriteLine(path);
                string contents = $"App stopping at {DateTime.Now}";
                File.AppendAllText(path, contents);

            }

            void OnAppStopped()
            {
                string path = $"{env.WebRootPath}/AppLog.txt";
                //Console.WriteLine(path);
                string contents = $"App stopped at {DateTime.Now}";
                File.AppendAllText(path, contents);
            }

        }



    }
}
