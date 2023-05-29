using System;
using System.Text.Json.Serialization;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarketplaceApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(c =>
                c.Conventions.Add(new SwaggerApplicationConvention()));

            // Register generator and it's dependencies

            services.AddSwaggerGen();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            var connectionString = "Host=localhost;Port=5432;Database=Marketplace;Username=postgres;Password=mypassword";


            services.AddDbContext<MarketplaceContext>(options => options.UseNpgsql(connectionString));
            
            /*
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
                */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
            
            app.UseSwagger();

            app.UseSwaggerUI(c=> 
            { 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "�� �� V1");
            });
            
            using (var serviceScope = app.ApplicationServices
                       .GetRequiredService<IServiceScopeFactory>()
                       .CreateScope())
            {
                try
                {
                    using (var context = serviceScope.ServiceProvider.GetService<MarketplaceContext>())
                    {
                        context.Database.Migrate();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}