
using System.Linq;
using BookingManagementSystem.Errors;
using BookingManagementSystem.Helpers.MappingProfile;
using Infrastructure.ApplicationContext;
using Infrastructure.Interfaces.IUnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingManagementSystem", Version = "v1" });
            });


            //DB Connection
            services.AddDbContext<BookingSystemApplicationContext>
                (opt => opt.UseSqlServer(Configuration.GetConnectionString("BookingManagementSystemDev")));

            //IUnitOfWork Config
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //AutoMapper Config
            services.AddAutoMapper(typeof(ApplicationMappingProfile));

            //Overriding ApiController ModelState Default Behavior
            services.Configure<ApiBehaviorOptions>(opt => {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                                 .SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray();

                    return new BadRequestObjectResult(new ApiValidationResponse { Errors = errors });

                };

            });


            //CORS Policy
            services.AddCors(o => o.AddPolicy("allowAll",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() ));


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

            app.UseCors("allowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
