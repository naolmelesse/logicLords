using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Pages.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Pages.Shared
{
    /// <summary>
    /// Unit tests for the LeaderboardSnippetModel class.
    /// </summary>
    public class LeaderboardSnippetModelTests
    {

        // The Compare Player Page Model instance being tested
        private readonly LeaderboardSnippetModel pageModel;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        public LeaderboardSnippetModelTests()
        {
            // Initialize the LeaderboardSnippetModel
            pageModel = new LeaderboardSnippetModel()
            {
            };
        }

        /// <summary>
        /// Verifies that setting and retrieving the Title property works as expected.
        /// </summary>
        [Test]
        public void Title_Valid_Setting_Title_should_be_able_to_retreive_title()
        {
            // Arrange

            // Expected title to set in the model
            var expectedTitle = "Top Players";

            // Act

            // Set the title in the model
            pageModel.Title = expectedTitle;

            // Assert
            Assert.That(expectedTitle, Is.EqualTo(pageModel.Title));
        }

        /// <summary>
        /// Verifies that setting and retrieving the Players property works as expected.
        /// </summary>
        [Test]
        public void Players_Valid_Setting_Players_should_be_able_to_retreive_Players()
        {
            // Arrange

            // Collection of players to set in the model
            var players = TestHelper.ProductService.GetAllData().Take(2);

            // Act

            // Set the players in the model
            pageModel.Players = players;

            // Assert
            Assert.That(players, Is.EqualTo(pageModel.Players));
        }

        /// <summary>
        /// Verifies that setting and retrieving the ShowMoreUrl property works as expected.
        /// </summary>
        [Test]
        public void ShowMoreUrl_Valid_Setting_ShowMore_should_be_able_to_retreive_ShowMoreUrl()
        {
            // Arrange

            // Expected URL to set in the model
            var expectedUrl = "/leaderboard/showmore";

            // Act

            // Set the URL in the model
            pageModel.ShowMoreUrl = expectedUrl;

            // Assert
            Assert.That(expectedUrl, Is.EqualTo(pageModel.ShowMoreUrl));
        }

        /// <summary>
        /// Verifies that setting and retrieving the DisplayStat property works as expected.
        /// </summary>
        [Test]
        public void DisplayStat_Valid_Setting_DisplayStat_should_be_able_to_retreive_DisplayStat()
        {
            // Arrange
            var expectedStat = "Goals";

            // Act

            // Set the display statistic in the model
            pageModel.DisplayStat = expectedStat;

            // Assert
            Assert.That(expectedStat, Is.EqualTo(pageModel.DisplayStat));
        }

        /// <summary>
        /// Tests GetPropertyValue method when a valid property is provided.
        /// </summary>
        [Test]
        public void GetPropertyValue_Valid_When_Provided_Goal_Property_Should_Return_Correct_Value()
        {
            // Arrange

            // Example player data for the test
            var player = TestHelper.ProductService.GetAllData().First();

            // Act

            // Retrieve the value of the Goals property
            var result = pageModel.GetPropertyValue(player, nameof(player.Goals));

            // Assert
            Assert.That(result, Is.EqualTo(player.Goals));
        }

        /// <summary>
        /// Tests GetPropertyValue method when an invalid property is provided.
        /// </summary>
        [Test]
        public void GetPropertyValue_Invalid_When_Property_Does_Not_Exist_Should_Return_Null()
        {
            // Arrange

            // Create a new product model with no properties initialized
            var player = new ProductModel();

            // Act

            // Attempt to retrieve a non-existent property
            var result = pageModel.GetPropertyValue(player, "NonExistentProperty");

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}