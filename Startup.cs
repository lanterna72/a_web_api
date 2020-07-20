using CoreNLogText;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace a_web_api
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    //policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    policy.SetIsOriginAllowedToAllowWildcardSubdomains()
                      .WithOrigins("https://*.cargostart.net", "https://*.cargostart.tech")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .Build();
                });
            });


            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();

            
            services.AddHealthChecks().AddCheck<ApiHealthCheck>("api");

            services.AddSingleton<ILog, LogNLog>();

            services.AddSignalR();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                //that's to the method you created 
                ResponseWriter = WriteHealthCheckResponse
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationhub");
                endpoints.MapControllers();
            });

        }

        private static Task WriteHealthCheckResponse(HttpContext httpContext,
 HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject(
            new JProperty("status", result.Status.ToString()),
            new JProperty("results", new JObject(result.Entries.Select(pair =>
            new JProperty(pair.Key, new JObject(
            new JProperty("status", pair.Value.Status.ToString()),
            new JProperty("description", pair.Value.Description),
            new JProperty("data", new JObject(pair.Value.Data.Select(
            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
            json.ToString(Formatting.Indented));
        }
    }
}
