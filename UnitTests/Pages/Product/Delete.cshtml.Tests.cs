using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    public class DeleteModelTests
    {
        public static DeleteModel pageModel;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

        [SetUp]
        public void TestInitialize()
        {

            pageModel = new DeleteModel(TestHelper.ProductService)
            {
            };
        }

        [Test]
        public void OnGet_ValidId_ShouldReturnPageResult()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var data = players.First(); // Take the first player from the JSON data

            // Act
            var result = pageModel.OnGet(data.Id) as PageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(pageModel.Player, Is.Not.Null);
            Assert.That(pageModel.Player.Id, Is.EqualTo(data.Id));
            Assert.That(pageModel.Player.Title, Is.EqualTo(data.Title));
        }

        [Test]
        public void OnGet_InvalidId_ShouldRedirectToErrorPage()
        {
            // Arrange
            var data = "invalid_id";


            // Act
            var result = pageModel.OnGet(data);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToPageResult>());
        }

        [Test]
        public void OnPost_ValidId_ShouldRedirectToIndexPage()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var validPlayer = players.First(); // Take the first player from the JSON data

            // Act
            var result = pageModel.OnPost(validPlayer.Id) as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageName, Is.EqualTo("Index"));

            // Ensure the player was deleted from TestHelper.ProductService
            var remainingPlayers = TestHelper.ProductService.GetAllData();
            Assert.That(remainingPlayers.Any(p => p.Id == validPlayer.Id), Is.False);
        }

        [Test]
        public void OnPost_InvalidId_ShouldRedirectToErrorPage()
        {
            // Arrange
            var invalidId = "nonexistent_id";

            // Act
            var result = pageModel.OnPost(invalidId) as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageName, Is.EqualTo("/Error"));
        }
    }
}