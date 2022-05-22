using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIMagicK_AWS.Data;
using APIMagicK_AWS.Helpers;
using APIMagicK_AWS.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace APIMagicK_AWS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowOrigin", x => x.AllowAnyOrigin()));
            string azureSql =
                        Configuration.GetConnectionString("azureSql");
            services.AddTransient<RepositoryUsuarios>();
            services.AddTransient<RepositoryItems>();
            services.AddDbContext<AzureContext>
                (options => options.UseMySql(azureSql,ServerVersion.AutoDetect(azureSql)));
            HelperOAuthToken helper = new HelperOAuthToken(Configuration);
            services.AddAuthentication(helper.GetAuthenticationOptions())
                .AddJwtBearer(helper.GetJwtOptions());
            services.AddTransient<HelperOAuthToken>
                (x => helper);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(options => options.AllowAnyOrigin());
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
            });
        }
    }
}
