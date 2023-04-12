using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingManagementSystem.Helpers.MappingProfile;
using Infrastructure.ApplicationContext;
using Infrastructure.Interfaces.IUnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OnlineShopWebAPIs.BusinessLogic.UnitOfWork;

namespace BookingManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {


            //DB Connection
            services.AddDbContext<BookingSystemApplicationContext>
                (opt => opt.UseSqlServer(Configuration.GetConnectionString("BookingManagementSystemDev")));

            //IUnitOfWork Config
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //AutoMapper Config
            services.AddAutoMapper(typeof(ApplicationMappingProfile));




            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingManagementSystem", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingManagementSystem v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
