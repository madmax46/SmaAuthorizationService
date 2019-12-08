using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DbWrapperCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlWrapper;
using SmaAuthorizationService.Repositories;
using SmaAuthorizationService.Repositories.Interfaces;
using SmaAuthorizationService.Services;
using SmaAuthorizationService.Services.Interfaces;

namespace SmaAuthorizationService
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
            services.AddControllers();
            services.AddLogging(logging =>
            {
            });

            services.AddSingleton<IConfigurationRoot>(serviceProvider =>
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var config = builder.Build();
                return config;
            });

            services.AddSingleton<MySqlConfig>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfigurationRoot>();
                return config.GetSection("MySql")?.Get<MySqlConfig>();
            });

            services.AddSingleton<IDBProvider>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<MySqlConfig>();
                return new MySqlWrap(config);
            });

            services.AddSingleton<IAuthenticateRepository, AuthenticateRepository>();
            services.AddSingleton<IAuthenticateService, AuthenticateService>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
