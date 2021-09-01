using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParkyAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.DataAccess.Repository.IRepository;
using ParkyAPI.DataAccess.Repository;
using ParkyAPI.Mapper;
using System.Reflection;
using System.IO;

namespace ParkyAPI
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration">Configuration model</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Startup configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// Services configuration method
        /// </summary>
        /// <param name="services">Container collection of services</param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>((options) => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(ParkyMappings));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("NPv1", new OpenApiInfo { Title = "ParkyAPI National Park", Version = "v1" });
                c.SwaggerDoc("Trailsv1", new OpenApiInfo { Title = "ParkyAPI Trails", Version = "v1" });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                c.IncludeXmlComments(cmlCommentsFullPath);
            });
        }
        /// <summary>
        /// Configure method
        /// </summary>
        /// <param name="app">Application</param>
        /// <param name="env">Environment</param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/NPv1/swagger.json", "ParkyAPI NP v1");
                    c.SwaggerEndpoint("/swagger/Trailsv1/swagger.json", "ParkyAPI Trails v1");
                });
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
