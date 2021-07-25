using CustomerMVCSite.Options;
using CustomerMVCSite.Services;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerMVCSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.AddOptions();

            services.Configure<CloudinaryOptions>(
                Configuration.GetSection("Cloudinary"));

            services.Configure<LocalUploadOptions>(
                Configuration.GetSection("LocalUpload"));

            // Services
            //Temporary lock cloudinary
            //services.AddScoped<IUploadService, CloudinaryService>();
            services.AddScoped<IUploadService, LocalUploadService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<eCommerceNetCoreContext>(_ => new eCommerceNetCoreContext());
            services.AddControllersWithViews();

            // Authentication
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
