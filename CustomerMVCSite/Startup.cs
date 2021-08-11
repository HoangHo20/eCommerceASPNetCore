using CustomerMVCSite.Options;
using CustomerMVCSite.Services;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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

            //services.AddIdentity<IdentityUser, Role>()
            //    .AddUserStore<UserStore>()
            //    .AddRoleStore<RoleStore>()
            //    .AddDefaultTokenProviders();

            //services.AddMemoryCache();
            //services.AddDistributedMemoryCache();
            //services.AddDbContext<IdentityServerDb>
            //    (options => options.UseSqlServer(Configuration.GetConnectionString("IdentityServerDb")));


            // Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultScheme = "Cookies";
                auth.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies", options =>
                {
                    options.Cookie.IsEssential = true;
                })
                .AddOpenIdConnect("oidc", opt =>
                {
                    opt.SignInScheme = "Cookies";
                    opt.Authority = "https://localhost:5005";
                    opt.ClientId = "mvc-client";
                    opt.ResponseType = "code";
                    opt.SaveTokens = true;
                    opt.ClientSecret = "MVCSecret";

                    opt.RequireHttpsMetadata = false;
                });

            // Services
            //Temporary lock cloudinary
            services.AddScoped<IUploadService, CloudinaryService>();
            //services.AddScoped<IUploadService, LocalUploadService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<ICastService, CastService>();
            services.AddScoped<eCommerceNetCoreContext>(_ => new eCommerceNetCoreContext(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options => options.AddPolicy("CorsMyPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllersWithViews();

            // Web Api
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "coreApi", Version = "v1" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "coreApi v1"));
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseCors("CorsMyPolicy");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "api/{controller}");

            });
        }
    }
}
