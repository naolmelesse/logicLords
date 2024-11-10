using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite
{

    /// <summary>
    /// The Startup class is responsible for configuring the application's services and HTTP request pipeline.
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Provides access to configuration settings.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Gets the application's configuration settings.
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services that the application will use.
        /// This method is called by the runtime during application startup.
        /// </summary>
        /// <param name="services">The service collection to which services are added.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddServerSideBlazor();
            services.AddHttpClient();
            services.AddControllers();
            services.AddTransient<JsonFileProductService>();
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// This method is called by the runtime to set up middleware components.
        /// </summary>
        /// <param name="app">Provides mechanisms to configure the application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();

                // endpoints.MapGet("/products", (context) => 
                // {
                //     var products = app.ApplicationServices.GetService<JsonFileProductService>().GetProducts();
                //     var json = JsonSerializer.Serialize<IEnumerable<Product>>(products);
                //     return context.Response.WriteAsync(json);
                // });
            });
        }
    }
}