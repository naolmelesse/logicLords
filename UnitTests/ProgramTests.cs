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

        #region Main Method Tests

        [Test]
        public void Main_Valid_Should_Run_WithoutException()
        {
            // Arrange
            string[] args = { };

            MethodInfo mainMethod = typeof(Program).GetMethod("Main", BindingFlags.Static | BindingFlags.Public);

            Assert.That(mainMethod, Is.Not.Null, "Main method should exist in Program class.");

            // Act
            Exception thrownException = null;
            Task mainTask = Task.Run(() =>
            {
                try
                {
                    // Invoke the Main method
                    mainMethod.Invoke(null, new object[] { args });
                }
                catch (TargetInvocationException tie)
                {
                    // Unwrap the inner exception
                    thrownException = tie.InnerException;
                }
            });

            // Allow the Main method to run for a short duration
            Thread.Sleep(2000);

            // Since Run() is blocking, we cannot gracefully stop the host.
            // We'll check if any exceptions were thrown during the invocation.

            // Assert
            Assert.That(thrownException, Is.Null, $"Main method threw an exception: {thrownException?.Message}");
        }

        #endregion Main Method Tests
    }
}
