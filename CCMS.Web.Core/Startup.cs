using CCMS.Application.EventSubscriber;
using CCMS.Application.Hubs;
using CCMS.Application.Options;
using Dapper;
using Furion;
using Furion.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CCMS.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddConfigurableOptions<AppDatabaseConfig>();
            services.AddConfigurableOptions<CacheOptions>();
            services.AddConfigurableOptions<Application.Options.JWTSettingsOptions>();
            
            services.AddConfigurableOptions<RefreshTokenSettingOptions>();
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);



            services.AddCorsAccessor();

            var dbSettings = App.GetOptions<AppDatabaseConfig>();
            var connectionString = dbSettings.GetConnectinStringActive();
          if (dbSettings.active==0)
            {
                services.AddDapper(connectionString, SqlProvider.Sqlite);
            }
            else if (dbSettings.active == 1)
            {
                services.AddDapper(connectionString, SqlProvider.SqlServer);
            }



            services.AddControllers()
                    .AddInjectWithUnifyResult();

            //    services.AddAuthorization(options => options.AddPolicy("Role",
            //policy => policy.AddRequirements(new RoleRequirement())
            //));
            services.AddSignalR();

            //services.AddCorsAccessor(options =>
            //{
            //    options.AddPolicy("ClientPermission", policy =>
            //    {
            //        policy.AllowAnyHeader()
            //            .AllowAnyMethod()
            //             .AllowAnyOrigin();
            //        // .WithOrigins("http://localhost:5000")
            //        // .AllowCredentials();
            //    });
            //});
            services.AddRemoteRequest();
            services.AddEventBus(builder =>
            {
                 
                builder.AddSubscriber<LogEventSubscriber>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseUnifyResultStatusCodes();
            app.UseHttpsRedirection();
          //  app.UsePathBase(new PathString("/api"));
            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app
            .UseInject(string.Empty,configure: options =>
             {
                 options.SpecificationDocumentConfigure = spt =>
                 {
                     spt.SwaggerConfigure = swg =>
                     {
                         // Swagger Options
                     };
                     spt.SwaggerUIConfigure = ui =>
                     {
                         // Swagger UI
                         ui.DefaultModelsExpandDepth(-1);
                     };
                 };
             });
 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UserHub>("/hubs/userhub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}