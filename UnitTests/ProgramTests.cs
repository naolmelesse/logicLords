using NUnit.Framework;
using ContosoCrafts.WebSite;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ProgramTests
    {
        #region CreateHostBuilder Tests

        [Test]
        public void CreateHostBuilder_Valid_Args_Should_Return_IHostBuilder()
        {
            // Arrange
            string[] args = { };

            // Act
            var hostBuilder = Program.CreateHostBuilder(args);

            // Assert
            Assert.That(hostBuilder, Is.Not.Null, "CreateHostBuilder should return a non-null IHostBuilder instance.");
            Assert.That(hostBuilder, Is.InstanceOf<IHostBuilder>(), "CreateHostBuilder should return an instance of IHostBuilder.");
        }

        [Test]
        public void CreateHostBuilder_Invalid_NullArgs_Should_Return_IHostBuilder()
        {
            // Arrange
            string[] args = null;

            // Act
            var hostBuilder = Program.CreateHostBuilder(args);

            // Assert
            Assert.That(hostBuilder, Is.Not.Null, "CreateHostBuilder should handle null arguments and return a non-null IHostBuilder.");
            Assert.That(hostBuilder, Is.InstanceOf<IHostBuilder>(), "CreateHostBuilder should return an instance of IHostBuilder even with null arguments.");
        }

        #endregion CreateHostBuilder Tests
    }
}
