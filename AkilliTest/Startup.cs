using AkilliTest.Helpers;
using Data.Contexts;
using Data.Contexts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AkilliTest.Business.Interfaces;
using AkilliTest.Business;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Data.Models;

namespace AkilliTest
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

            services.AddScoped<IAkilliTestBusiness, AkilliTestBusiness>();

            services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(GetConnectionString()));
            //services.AddScoped<IBaseUnitOfWork<BaseDbContext>, BaseUnitOfWork<BaseDbContext>>();

            services.AddScoped<IBaseRepository<Product>, BaseRepository<Product>>();
            services.AddScoped<IBaseRepository<Category>, BaseRepository<Category>>();
            services.AddScoped<IBaseRepository<Order>, BaseRepository<Order>>();
            services.AddScoped<IBaseRepository<OrderProduct>, BaseRepository<OrderProduct>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
