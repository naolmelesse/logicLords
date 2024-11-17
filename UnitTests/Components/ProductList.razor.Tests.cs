using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnitTests.Components
{
    /// <summary>
    /// Unit tests for productList
    /// </summary>
    public class ProductListTests : BunitTestContext
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
        /// Tests if player names are displayed when the component renders with a list of players.
        /// </summary>
        [Test]
        public void RenderComponent_With_Players_Players_Exist_Should_Display_Player_Names()
        {

            // Arrange

            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Get all player data from the ProductService.
            var players = TestHelper.ProductService.GetAllData();


            // Act

            // Render the ProductList component and pass in the players list as a parameter.
            var page = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Players, players));

            // Retrieve the markup of the rendered component.
            var result = page.Markup;


            // Assert

            // Verify that the player name "Alonso Martinez" is present in the markup.
            Assert.That(result.Contains("Alonso Martinez"));
        }

        /// <summary>
        /// Tests if a message is displayed when there are no players in the component.
        /// </summary>
        [Test]
        public void RenderComponent_With_No_Players_Empty_List_Should_Display_No_Players_Message()
        {

            // Arrange

            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);


            // Act

            // Render the ProductList component and pass an empty list of players as a parameter.
            var page = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Players, new List<ProductModel>()));

            // Retrieve the markup of the rendered component.
            var result = page.Markup;


            // Assert

            // Verify that the "No players available to display." message is present in the markup.
            Assert.That(result.Contains("No players available to display."));
        }

        #endregion Rendering Tests

        #region Interaction Tests

        /// <summary>
        /// Tests if submitting a rating adds the rating to the player's data.
        /// </summary>
        [Test]
        public void SubmitRating_No_Initial_Rating_Click_Star_Should_Add_Rating_To_Player()
        {

            // Arrange

            // Get all player data from the ProductService.
            var players = TestHelper.ProductService.GetAllData();

            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Render the ProductList component and pass in the players list as a parameter.
            var page = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Players, players));

            // Find the first player with no ratings.
            var noRatingsPlayer = players.First(p => p.Ratings == null);


            // Act

            // Click the "more" button for the selected player to open the modal.
            page.Find("#morebutton_" + noRatingsPlayer.Id).Click();
            page.Find("#morebutton_" + noRatingsPlayer.Id).Click(); // Open modal

            // Click the third star to submit a rating of 3.
            page.FindAll("span.fa-star")[2].Click();


            // Assert

            // Retrieve the updated player data after the rating submission.
            var updatedPlayer = TestHelper.ProductService.GetAllData().First(p => p.Id == players.First().Id);

            // Verify that the rating of 3 was added to the player's data.
            Assert.That(updatedPlayer.Ratings.Contains(3));
        }

        /// <summary>
        /// Tests if clicking a button displays the player's modal with detailed information.
        /// </summary>
        [Test]
        public void SelectProduct_With_Valid_Player_Id_Click_Button_Should_Display_Player_Modal()
        {

            // Arrange

            // Get all player data from the ProductService.
            var players = TestHelper.ProductService.GetAllData();

            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Render the ProductList component and pass in the players list as a parameter.
            var page = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Players, players));


            // Act

            // Click the "more" button for the first player to open the modal.
            page.Find("#morebutton_" + players.First().Id).Click();
            page.Find("#morebutton_" + players.First().Id).Click();

            // Retrieve the markup of the rendered component.
            var result = page.Markup;


            // Assert

            // Verify that the "Current team" text is present in the modal markup.
            Assert.That(result.Contains("Current team"));
        }

        /// <summary>
        /// Tests if a single vote count is displayed when a player has one rating.
        /// </summary>
        [Test]
        public void PlayerVotes_Single_Vote_Click_Button_Should_Display_Single_Vote_Count()
        {

            // Arrange

            // Get all player data from the ProductService.
            var players = TestHelper.ProductService.GetAllData();

            // Set a single rating of 5 for the first player.
            var singleVotePlayer = players.First();
            singleVotePlayer.Ratings = new List<int> { 5 }.ToArray();

            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Render the ProductList component and pass in the players list as a parameter.
            var page = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Players, players));


            // Act

            // Click the "more" button for the player to open the modal.
            page.Find("#morebutton_" + singleVotePlayer.Id).Click();
            page.Find("#morebutton_" + singleVotePlayer.Id).Click();

            // Retrieve the markup of the rendered component.
            var result = page.Markup;


            // Assert

            // Verify that the "1 Vote" text is present in the modal markup.
            Assert.That(result.Contains("1 Vote"));
        }

        /// <summary>
        /// Tests if multiple vote counts are displayed correctly for a player with multiple ratings.
        /// </summary>
        [Test]
        public void PlayerVotes_Multiple_Votes_Click_Button_Should_Display_Multiple_Vote_Count()
        {

            // Arrange

            // Get all player data from the ProductService.
            var players = TestHelper.ProductService.GetAllData();

            // Set multiple ratings for the first player.
            var multipleVotesPlayer = players.First();
            multipleVotesPlayer.Ratings = new List<int> { 4, 5, 3 }.ToArray();

            // Add the ProductService to the test's service collection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Render the ProductList component and pass in the players list as a parameter.
            var page = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Players, players));


            // Act

            // Click the "more" button for the player to open the modal.
            page.Find("#morebutton_" + multipleVotesPlayer.Id).Click();
            page.Find("#morebutton_" + multipleVotesPlayer.Id).Click();

            // Retrieve the markup of the rendered component.
            var result = page.Markup;


            // Assert

            // Verify that the "3 Votes" text is present in the modal markup.
            Assert.That(result.Contains("3 Votes"));
        }

        [Test]
        public void SubmitRating_With_Valid_Rating_Should_Display_Checked_Stars()
        {

            // Arrange

            // Get all player data from the ProductService.
            var players = TestHelper.ProductService.GetAllData();

            // Select the first player to rate.
            var playerToRate = players.First();

            // Pre-set the player's ratings to include a rating of 5.
            playerToRate.Ratings = new List<int> { 5 }.ToArray();

            // Add the ProductService to the test's service collection for dependency injection.
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Render the ProductList component, passing the list of players as a parameter.
            var page = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Players, players));


            // Act

            // Open the modal for the selected player by clicking the "more" button.
            page.Find("#morebutton_" + playerToRate.Id).Click();

            // Submit a rating of 4 by clicking the fourth star.
            page.FindAll("span.fa-star")[3].Click();


            // Assert

            // Retrieve the markup of the rendered component after rating submission.
            var result = page.Markup;

            // Verify that the text "4 Votes" is present in the markup, indicating the rating submission.
            Assert.That(result.Contains("4 Votes"));

            // Verify that a star is marked as checked, indicating the selected rating is displayed.
            Assert.That(result.Contains("fa-star checked"));
        }

        #endregion Interaction Tests
    }
}