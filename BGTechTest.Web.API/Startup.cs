using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Repositories;
using BGTechTest.Web.API.Helpers;
using BGTechTest.Web.API.Service;
using BGTechTest.Web.API.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BGTechTest.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        public IHostingEnvironment CurrentEnvironment { get;}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IIdentityNumberValidator, IdentityNumberValidator>();
            services.AddScoped<IDataSerializer, CsvSerializer>();
            services.AddScoped<IDataRepository, Csvrepository>();
            services.AddScoped<IIdentityNumberService, IdentityNumberService>();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
