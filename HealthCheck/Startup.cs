﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthCheck.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HealthCheck
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
            services.AddHealthChecks(checks =>
            {
                checks.AddUrlCheck("https://github.com")
               .AddSqlCheck("Local DB Check", "Server=(localdb)\\mssqllocaldb;Database=aspnet-WebApplication1;Trusted_Connection=True;MultipleActiveResultSets=true");
               //.AddHealthCheckGroup("performance",
               //    group => group.AddPrivateMemorySizeCheck(1000)
               //        .AddVirtualMemorySizeCheck(1000)
               //        .AddWorkingSetCheck(1000)
               //        .AddCheck<CDriveHasMoreThan1GbFreeHealthCheck>("C Drive has more than 1 GB Free"),
               //    CheckStatus.Unhealthy
               //);
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}
