using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages
{
    public class ComparePlayers
    {
        #region TestSetup

        public static ComparePlayersModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            var MockLoggerDirect = Mock.Of<ILogger<ComparePlayersModel>>();

            pageModel = new ComparePlayersModel(MockLoggerDirect, TestHelper.ProductService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_No_Selected_Player_Id_Return_Void()
        {
            // Arrange
            pageModel.SelectedPlayerLeftId = null;
            pageModel.SelectedPlayerRightId = null;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.SelectedPlayerRight, Is.Null);
            Assert.That(pageModel.SelectedPlayerLeft, Is.Null);
        }

        [Test]
        public void OnGet_Valid_Left_Side_Player_Selected_Return_Void()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var data = TestHelper.ProductService.GetAllData().First();
            pageModel.SelectedPlayerLeftId = data.Id;
            pageModel.SelectedPlayerRightId = null;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.SelectedPlayerLeft.Id, Is.EqualTo(data.Id));
            Assert.That(pageModel.SelectedPlayerRight, Is.Null);
        }


        [Test]
        public void OnGet_Valid_Right_Side_Player_Selected_Return_Void()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var data = TestHelper.ProductService.GetAllData().Skip(1).First();
            pageModel.SelectedPlayerLeftId = null;
            pageModel.SelectedPlayerRightId = data.Id;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.SelectedPlayerLeft, Is.Null);
            Assert.That(pageModel.SelectedPlayerRight.Id, Is.EqualTo(data.Id));
        }

        [Test]
        public void OnGet_Valid_Both_Sides_Player_Selected_Return_Void()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            var data1 = TestHelper.ProductService.GetAllData().Skip(2).First();
            var data2 = TestHelper.ProductService.GetAllData().Skip(3).First();
            pageModel.SelectedPlayerLeftId = data1.Id;
            pageModel.SelectedPlayerRightId = data2.Id;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.SelectedPlayerLeft.Id, Is.EqualTo(data1.Id));
            Assert.That(pageModel.SelectedPlayerRight.Id, Is.EqualTo(data2.Id));
        }

        [Test]
        public void PlayerDropDownItems_Property_Should_Be_Assignable_And_Retrievable()
        {
            // Arrange
            var testPlayerList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Player One" },
                new SelectListItem { Value = "2", Text = "Player Two" }
            };

            // Act
            pageModel.PlayerDropDownItems = testPlayerList;
            var result = pageModel.PlayerDropDownItems;

            // Assert
            Assert.That(result, Is.EqualTo(testPlayerList), "PlayerDropDownItems should return the assigned list.");
        }
        #endregion OnGet
    }
}

