using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var result1 = pageModel.SelectedPlayerRight;
            var result2 = pageModel.SelectedPlayerLeft;

            // Assert
            Assert.That(result1, Is.Null);
            Assert.That(result2, Is.Null);
        }

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

