using AkilliTest.Helpers;
using Data.Contexts;
using Data.Contexts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MigrationToolTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

            var webSettings = new DBConfigHelper();
            config.GetSection("DBConfig").Bind(webSettings);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(GetConnectionString(), b => b.MigrationsAssembly("MigrationToolTest")));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }

        private string GetConnectionString()
        {

            var address = (DBConfigHelper.Address);
            var name = (DBConfigHelper.DbName);
            var username = (DBConfigHelper.Username);
            var password = (DBConfigHelper.Password);

            //var conStr = $"Server={address};Host={address};Database={name};User Id={username};Password={password};";//for psql

            var conStr = $"data source={address};initial catalog={name};persist security info=True;user id={username};password={password};MultipleActiveResultSets=True;App=EntityFramework";
            return conStr;
        }
    }
}
