using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;


namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for DeleteModel functionality.
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
            pageModel = new DeleteModel(TestHelper.ProductService);
        }

        #region OnGet

        /// <summary>
        /// Test case for fetching a player with a missing ID, expecting redirection to the Error page.
        /// </summary>
        [Test]
        public void OnGet_Missing_Id_Should_Redirect_To_Error_Page()
        {
            // Arrange
            pageModel.Id = null; // Set the ID to null

            // Act
            var result = pageModel.OnGet();


            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToPageResult>());
            Assert.That((result as RedirectToPageResult).PageName, Is.EqualTo("/Error"));
        }

        /// <summary>
        /// Test case for fetching a player with an invalid ID, expecting redirection to the Error page.
        /// </summary>
        [Test]
        public void OnGet_Invalid_Nonexistent_Id_Should_Redirect_To_Error_Page()
        {
            // Arrange
            pageModel.Id = "invalid_id"; // Define an invalid player ID

            // Act
            var result = pageModel.OnGet();

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
            var players = TestHelper.ProductService.GetAllData();
            var data = players.First(); // Select the first player from the JSON data
            pageModel.Id = data.Id;

            // Act
            PageResult result = pageModel.OnGet() as PageResult;

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
            pageModel.Id = "nonexistent_id"; // Define a nonexistent player ID

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToPageResult>());
            Assert.That((result as RedirectToPageResult).PageName, Is.EqualTo("/Error"));
        }

        /// <summary>
        /// Test case for deleting a valid player by ID, expecting redirection to the Index page after deletion.
        /// </summary>
        [Test]
        public void OnPost_Valid_Id_Should_Redirect_To_Index_Page_And_Player_Is_Deleted()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var validPlayer = players.First(); // Select the first player from the JSON data
            pageModel.Id = validPlayer.Id;

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToPageResult>());
            Assert.That((result as RedirectToPageResult).PageName, Is.EqualTo("/Product/Index"));

            // Ensure the player was deleted from TestHelper.ProductService
            var remainingPlayers = TestHelper.ProductService.GetAllData();
            Assert.That(remainingPlayers.Any(p => p.Id == validPlayer.Id), Is.False);
        }

        #endregion OnPost
    }
}