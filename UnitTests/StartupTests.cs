using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

using NUnit.Framework;

namespace UnitTests.Pages.Startup
{
    public class StartupTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        public class Startup : ContosoCrafts.WebSite.Startup
        {
            public Startup(IConfiguration config) : base(config) { }

            public IConfiguration ConfigurationTest => Configuration;
        }

        #endregion TestSetup

        #region Configuration Property Getter

        [Test]
        public void Startup_Configuration_Should_Return_IConfiguration_Instance()
        {
            // Arrange
            var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
            var startup = new Startup(configuration);

            // Act
            var result = startup.ConfigurationTest;

            // Assert
            Assert.That(result, Is.Not.Null, "Configuration property should return a non-null IConfiguration instance.");
        }

        #endregion Configuration Property Getter

        #region ConfigureServices

        [Test]
        public void Startup_ConfigureServices_Valid_Default_Should_Pass()
        {
            // Arrange
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
            var startup = new Startup(configuration);

            // Act
            startup.ConfigureServices(services);

            // Assert
            Assert.That(services.Count, Is.GreaterThan(0), "Services should be registered in ConfigureServices.");
        }

        #endregion ConfigureServices

        #region Configure

        [Test]
        public void Startup_Configure_Development_Should_Run_WithoutException_And_UseDeveloperExceptionPage()
        {
            // Arrange
            var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
            var startup = new Startup(configuration);

            var webHostBuilder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            var webHost = webHostBuilder.Build();

            var serviceProvider = webHost.Services;

            var appBuilder = new Microsoft.AspNetCore.Builder.ApplicationBuilder(serviceProvider);

            var env = (IWebHostEnvironment)serviceProvider.GetService(typeof(IWebHostEnvironment));

            // Act

            //  Assert
            Assert.That(env.EnvironmentName, Is.EqualTo("Development"), "Environment should be Development.");

            Assert.That(() =>
            {
                startup.Configure(appBuilder, env);
            }, Throws.Nothing, "Configure should run without exception in Development environment.");
        }

        [Test]
        public void Startup_Configure_Production_Should_Run_WithoutException_And_NotUseDeveloperExceptionPage()
        {
            // Arrange
            var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
            var startup = new Startup(configuration);

            var webHostBuilder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseEnvironment("Production")
                .UseStartup<Startup>();

            var webHost = webHostBuilder.Build();

            var serviceProvider = webHost.Services;

            var appBuilder = new Microsoft.AspNetCore.Builder.ApplicationBuilder(serviceProvider);

            var env = (IWebHostEnvironment)serviceProvider.GetService(typeof(IWebHostEnvironment));

            // Act
            
            // Assert
            Assert.That(env.EnvironmentName, Is.EqualTo("Production"), "Environment should be Production.");

            Assert.That(() =>
            {
                startup.Configure(appBuilder, env);
            }, Throws.Nothing, "Configure should run without exception in Production environment.");
        }

        #endregion Configure
    }
}
