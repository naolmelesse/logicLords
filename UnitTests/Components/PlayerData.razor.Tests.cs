using System.Linq;
using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace UnitTests.Components
{
    public class PlayerDataTests : Bunit.TestContext
    {

        #region TestSetup

        /// <summary>
        /// Initializes the test setup before each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            
        }

        #endregion TestSetup

        #region Rendering Tests

        /// <summary>
        /// Test to ensure that PlayerData component displays correct data for a valid player ID.
        /// </summary>
        [Test]
        public void PlayerData_Valid_Id_Players_Displays_Correct_Data_Elements()
        {
            // Arrange
            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Get all player data from the ProductService.
            var players = TestHelper.ProductService.GetAllData();

            // Valid player the ProductService.
            var validPlayer = players.First();

            // Act

            // Render the PlayerData component with the valid player ID as a parameter
            var cut = RenderComponent<PlayerData>(parameters => parameters.Add(p => p.Id, validPlayer.Id));

            // Find the first <h1> element in the rendered component
            var heading = cut.Find("h1");

            // Find the first <img> element in the rendered component
            var image = cut.Find("img");

            // Find all <li> elements within an ordered list (<ol>) in the rendered component
            var listItems = cut.FindAll("ol li");

            // Assert
            Assert.That(heading.TextContent, Is.EqualTo(validPlayer.Title));
        }

        #endregion Rendering Tests
    }
}
