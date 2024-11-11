using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{

    /// <summary>
    /// Unit tests for Admin Delete functionality.
    /// </summary>
    public class DeleteModelTests
    {

        // Static instance of DeleteModel for testing.
        public static DeleteModel pageModel;

        /// <summary>
        /// Initializes test setup by creating an instance of DeleteModel with TestHelper's ProductService.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {

            pageModel = new DeleteModel(TestHelper.ProductService)
            {
            };
        }

        #region OnGet

        /// <summary>
        /// Test case for fetching a player with an invalid ID, expecting redirection to the Error page.
        /// </summary>
        [Test]
        public void OnGet_Invalid_Nonexistent_Id_Should_Redirect_To_Error_Page()
        {

            // Arrange

            // Define an invalid player ID
            var data = "invalid_id";


            // Act

            // Attempt to retrieve the page for the invalid ID
            var result = pageModel.OnGet(data);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToPageResult>());
            Assert.That((result as RedirectToPageResult).PageName, Is.EqualTo("/Error"));
        }

        /// <summary>
        /// Test case for fetching a valid player by ID, expecting the result to be a valid PageResult with matching player details.
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Return_PageResult()
        {

            // Arrange

            // Get all players from the ProductService
            var players = TestHelper.ProductService.GetAllData();

            // Select the first player from the JSON data
            var data = players.First();

            // Act

            // Attempt to retrieve the page for the selected player ID
            var result = pageModel.OnGet(data.Id) as PageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(pageModel.Player, Is.Not.Null);
            Assert.That(pageModel.Player.Id, Is.EqualTo(data.Id));
            Assert.That(pageModel.Player.Title, Is.EqualTo(data.Title));
        }

        #endregion OnGet

        #region OnPost

        /// <summary>
        /// Test case for attempting to delete a player with an invalid ID, expecting redirection to the Error page.
        /// </summary>
        [Test]
        public void OnPost_InvalidId_Should_Redirect_To_Error_Page()
        {

            // Arrange

            // Define a nonexistent player ID
            var invalidId = "nonexistent_id";

            // Act

            // Attempt to delete the player by the invalid ID
            var result = pageModel.OnPost(invalidId) as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageName, Is.EqualTo("/Error"));
        }


        /// <summary>
        /// Test case for deleting a valid player by ID, expecting redirection to the Index page after deletion.
        /// </summary>
        [Test]
        public void OnPost_Valid_Id_Should_Redirect_To_Index_Page_And_Player_Is_Deleted()
        {

            // Arrange

            // Get all players from the ProductService
            var players = TestHelper.ProductService.GetAllData();

            // Select the first player from the JSON data as a valid player
            var validPlayer = players.First(); // Take the first player from the JSON data

            // Act

            // Attempt to delete the player by ID
            var result = pageModel.OnPost(validPlayer.Id) as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageName, Is.EqualTo("Index"));

            // Ensure the player was deleted from TestHelper.ProductService
            var remainingPlayers = TestHelper.ProductService.GetAllData();
            Assert.That(remainingPlayers.Any(p => p.Id == validPlayer.Id), Is.False);
        }

        #endregion OnPost

    }
}