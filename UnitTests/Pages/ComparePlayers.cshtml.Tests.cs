using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages
{
    /// <summary>
    /// Class for unit tests of Compare players feature
    /// </summary>
    public class ComparePlayers
    {
        #region TestSetup

        // The Compare Player Page Model instance being tested
        public static ComparePlayersModel pageModel;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a mock logger for the ComparePlayersModel
            var MockLoggerDirect = Mock.Of<ILogger<ComparePlayersModel>>();

            // Initialize the ComparePlayersModel with the mock logger and test product service
            pageModel = new ComparePlayersModel(MockLoggerDirect, TestHelper.ProductService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test that OnGet with no player ID selected returns null
        /// </summary>
        [Test]
        public void OnGet_Valid_No_Selected_Player_Id_Return_Void()
        {
            // Arrange
            pageModel.SelectedPlayerLeftId = null;
            pageModel.SelectedPlayerRightId = null;

            // Act
            pageModel.OnGet();
            var result1 = pageModel.SelectedPlayerRight;
            var result2 = pageModel.SelectedPlayerLeft;

            // Assert
            Assert.That(result1, Is.Null);
            Assert.That(result2, Is.Null);
        }

        /// <summary>
        /// Test that OnGet with only the left player selected sets the left player correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Left_Side_Player_Selected_Return_Void()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var data = players.First().Id;
            pageModel.SelectedPlayerLeftId = data;
            pageModel.SelectedPlayerRightId = null;

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerLeft.Id;

            // Assert
            Assert.That(result, Is.EqualTo(data));
            Assert.That(pageModel.SelectedPlayerRight, Is.Null);
        }

        /// <summary>
        /// Test that OnGet with only the right player selected sets the right player correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Right_Side_Player_Selected_Return_Void()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var data = players.Skip(1).First().Id;
            pageModel.SelectedPlayerLeftId = null;
            pageModel.SelectedPlayerRightId = data;

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerRight.Id;

            // Assert
            Assert.That(result, Is.EqualTo(data));
            Assert.That(pageModel.SelectedPlayerLeft, Is.Null);
        }

        /// <summary>
        /// Test that OnGet with both players selected sets both players correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Both_Sides_Player_Selected_Return_Void()
        {
            // Arrange
            var player = TestHelper.ProductService.GetAllData();
            var data1 = player.Skip(2).First().Id;
            var data2 = player.Skip(3).First().Id;
            pageModel.SelectedPlayerLeftId = data1;
            pageModel.SelectedPlayerRightId = data2;

            // Act
            pageModel.OnGet();
            var result1 = pageModel.SelectedPlayerLeft.Id;
            var result2 = pageModel.SelectedPlayerRight.Id;

            // Assert
            Assert.That(result1, Is.EqualTo(data1));
            Assert.That(result2, Is.EqualTo(data2));
        }

        /// <summary>
        /// Test that the PlayerDropDownItems property can be assigned and retrieved correctly
        /// </summary>
        [Test]
        public void PlayerDropDownItems_Valid_Property_Should_Be_Assignable_And_Retrievable()
        {
            // Arrange
            var data = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Player One" },
                new SelectListItem { Value = "2", Text = "Player Two" }
            };

            // Act
            pageModel.PlayerDropDownItems = data;
            var result = pageModel.PlayerDropDownItems;

            // Assert
            Assert.That(result, Is.EqualTo(data), "PlayerDropDownItems should return the assigned list.");
        }

        #endregion OnGet
    }
}
