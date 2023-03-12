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

namespace Globomantics.ProductsApi
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
            // [Add] Allow Specific Origins
            /*services.AddCors(options => options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:8080", "http://localhost:8081")
            ));*/

            // [Add] Allow Specific Origins from environment
            var allowedOrigin = Configuration.GetValue<string>("AllowedOrigin") ?? "";
            services.AddCors(options => {
                options.AddPolicy("Private", builder => builder.WithOrigins(allowedOrigin).SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
                options.AddPolicy("Public", builder => builder.AllowAnyOrigin().WithMethods("Get").WithHeaders("Content-Type"));
                options.AddPolicy("AllowSubDomains", builder => builder.WithOrigins("https://*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains());
                options.AddPolicy("AllowMultipleDomains", builder => builder.SetIsOriginAllowed(checkWhitelistingDomain));
            }
                );
            services.AddControllers();
        }

        private bool checkWhitelistingDomain(string domain)
        {
            var whitelistingDomains = new[] { "globalmantics.com", "example.com" };
            return whitelistingDomains.Any(origin => domain.Contains(origin));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // [Add]  Allow Specific Origins
            app.UseCors("Private");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
