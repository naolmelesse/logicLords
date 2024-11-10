using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ContosoCrafts.WebSite

{
    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    public class Program
    {

        /// <summary>
        /// The Main method is the entry point of the application.
        /// It creates the host builder, builds it, and starts the web application.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Configures and returns an IHostBuilder instance.
        /// This sets up the default hosting configuration and specifies the Startup class.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        /// <returns>An IHostBuilder instance configured for the web application.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Specifies the Startup class, which contains the application's configuration.
                    webBuilder.UseStartup<Startup>();

                });
    }
}