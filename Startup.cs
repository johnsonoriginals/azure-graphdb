using Hackathon.Models;
using Hackathon.Resources;
using Hackathon.Services;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Hackathon
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

            services.AddControllersWithViews();

            var configurationSection = Configuration.GetSection("Gremlin");
            string container = configurationSection.GetSection("ContainerName").Value ?? throw new ArgumentException("Missing env var: ContainerName");
            string database = configurationSection.GetSection("DatabaseName").Value ?? throw new ArgumentException("Missing env var: DatabaseName");
            string apiKey = configurationSection.GetSection("Key").Value ?? throw new ArgumentException("Missing env var: PrimaryKey");
            string host = configurationSection.GetSection("Account").Value ?? throw new ArgumentException("Missing env var: Host");

            services.AddScoped<IGremlinWrapper>(sp => new GremlinWrapper(container, database, apiKey, host));

            services.AddScoped<IAsyncGremlinService, AsyncGremlinService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GraphDBApi", Version = "v1" });
            });

            services.AddJsonApi(
               options => options.Namespace = "jsonapi/v1",
               resources: builder => builder.Add<Posts, Guid>("posts")
           );

            services.AddScoped<IResourceService<Posts, Guid>, PostService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseJsonApi();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Post}/{action=Index}/{pk?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphDB API V1");
            });


        }
    }
}
