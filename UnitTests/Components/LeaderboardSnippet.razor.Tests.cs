using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnitTests;

namespace UnitTests.Components
{

    /// <summary>
    /// Unit tests for the LeaderboardSnippet component
    /// </summary>
    public class LeaderboardSnippetTests : BunitTestContext
    {
        #region TestSetup

        // Initializes the test setup
        [SetUp]
        public void TestInitialize()
        {
            // Add any necessary setup here
        }

        #endregion TestSetup

        #region Rendering Tests

        /// <summary>
        /// Test rendering the component with valid players. Should display player names and stats correctly
        /// </summary>
        [Test]
        public void RenderComponent_Valid_With_Players_Should_Display_Player_Names()
        {
            // Arrange

            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Retrieve the first player from the product service data, ordered by descending goals
            var players = TestHelper.ProductService.GetAllData().OrderByDescending(p => p.Goals).Take(5);

            // Act
            var page = RenderComponent<LeaderboardSnippet>(parameters => parameters
                .Add(p => p.Players, players)
                .Add(p => p.Title, "Top Goal Scorers")
                .Add(p => p.DisplayStat, "Goals")
                .Add(p => p.ShowMoreUrl, "/leaderboard/goalsleaderboard"));

            var renderedMarkup = page.Markup;

            // Assert
            Assert.That(renderedMarkup.Contains("Christian Benteke"));
            Assert.That(renderedMarkup.Contains("Goals: 20"));
        }

        #endregion Rendering Tests

        #region Interaction Tests

        /// <summary>
        /// Test the "Show More" link. Should navigate to the correct URL
        /// </summary>
        [Test]
        public void ClickShowMore_Valid_Should_Navigate_To_Correct_URL()
        {
            // Arrange
            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Retrieve the first player from the product service data, ordered by descending goals
            var players = TestHelper.ProductService.GetAllData().OrderByDescending(p => p.Goals).Take(5);
            
            // Define the URL for the "Show More" leaderboard page
            var showMoreUrl = "/leaderboard/goalsleaderboard";

            // Act
            var page = RenderComponent<LeaderboardSnippet>(parameters => parameters
                .Add(p => p.Players, players)
                .Add(p => p.Title, "Top Goal Scorers")
                .Add(p => p.DisplayStat, "Goals")
                .Add(p => p.ShowMoreUrl, showMoreUrl));
            var showMoreLink = page.Find("a.btn");


            // Assert
            Assert.That(showMoreLink.GetAttribute("href"), Is.EqualTo(showMoreUrl));
        }
        #endregion Interaction Tests

        #region GetPropertyValue

        /// <summary>
        /// Test GetPropertyValue with a valid property. Should return the correct value
        /// </summary>
        [Test]
        public void GetPropertyValue_Valud_With_Valid_Property_Should_Return_Correct_Value()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Retrieve the first player from the product service data, ordered by descending goals
            var player = TestHelper.ProductService.GetAllData().OrderByDescending(p => p.Goals).First();
            
            // Retrieve the number of goals scored by the player
            var goalsScored = player.Goals;

            // Initialize a new instance of the LeaderboardSnippet component
            var component = new LeaderboardSnippet();

            // Act
            var result = component.GetPropertyValue(player, "Goals");

            // Assert
            Assert.That(result, Is.EqualTo(goalsScored));
        }

        /// <summary>
        /// Test GetPropertyValue with an invalid property. Should return null
        /// </summary>
        [Test]
        public void GetPropertyValue_Invalid_With_Invalid_Property_Should_Returns_Null()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Retrieve the first player from the product service data, ordered by descending goals
            var player = TestHelper.ProductService.GetAllData().OrderByDescending(p => p.Goals).First();

            // Retrieve the number of goals scored by the player
            var goalsScored = player.Goals;

            // Initialize a new instance of the LeaderboardSnippet component
            var component = new LeaderboardSnippet();

            // Act
            var result = component.GetPropertyValue(player, "InvalidProperty");

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion GetPropertyValue
    }
}