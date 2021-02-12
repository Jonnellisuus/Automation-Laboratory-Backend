using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Automation_Laboratory_Backend.Models;
using Automation_Laboratory_Backend.Repositories;
using Automation_Laboratory_Backend.Services;
using Microsoft.CodeAnalysis.Host;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace Automation_Laboratory_Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "wwwroot";
            });

            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceService, DeviceService>();

            services.AddScoped<IWorkplaceRepository, WorkplaceRepository>();
            services.AddScoped<IWorkplaceService, WorkplaceService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AutomationlaboratorydbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("AzureConnectionString"));
            });

            var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "keys", "firebase_admin_sdk.json");

            if (HostingEnvironment.IsEnvironment("local"))
                pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "keys", "firebase_admin_sdk.local.json");

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(pathToKey)
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var firebaseProjectName = Configuration["konekortit"];
                    options.Authority = "https://securetoken.google.com/" + firebaseProjectName;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://securetoken.google.com/" + firebaseProjectName,
                        ValidateAudience = true,
                        ValidAudience = firebaseProjectName,
                        ValidateLifetime = true
                    };
                });

            services.AddControllers();

            services.AddMvc().AddNewtonsoftJson(json => json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // CORS policy starts here.

            services.AddCors(options => options.AddPolicy("AutomationLaboratoryPolicy",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }));

            // CORS policy stops here.

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors("AutomationLaboratoryPolicy");
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client-app";
                if (env.IsDevelopment() || env.IsEnvironment("local"))
                {
                    var startScript = env.IsEnvironment("local") ? "start-local" : "start";
                    spa.UseAngularCliServer(npmScript: startScript);
                }

            });
        }
    }
}
