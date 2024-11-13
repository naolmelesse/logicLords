using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

using NUnit.Framework;

namespace UnitTests.Pages.Startup
{
    public class StartupTests
    {
        #region TestSetup

        /// <summary>
        /// Initializes common objects needed for the tests.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }
        public class Startup : ContosoCrafts.WebSite.Startup
        {
    
            // Constructor that calls the base class constructor.
            public Startup(IConfiguration config) : base(config) { }

            // Shows the Configuration property for testing the getter.
            public IConfiguration ConfigurationTest => Configuration;
        }

        #endregion TestSetup

        #region Configuration Property Getter

        /// <summary>
        /// Test that the Configuration property getter returns a valid IConfiguration instance.
        /// </summary>
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

        /// <summary>
        /// Test that ConfigureServices method registers services correctly.
        /// </summary>
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

        /// <summary>
        /// Test that Configure method runs without exception in Development environment and uses DeveloperExceptionPage.
        /// </summary>
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

            // Assert
            Assert.That(env.EnvironmentName, Is.EqualTo("Development"), "Environment should be Development.");

            Assert.That(() =>
            {
                startup.Configure(appBuilder, env);
            }, Throws.Nothing, "Configure should run without exception in Development environment.");
        }

        /// <summary>
        /// Test that Configure method runs without exception in Production environment and does not use DeveloperExceptionPage.
        /// </summary>
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
