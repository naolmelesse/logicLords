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

            // result of selected player right
            var result1 = pageModel.SelectedPlayerRight;

            // result of selected player right
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

            // Retrieve all player data using the ProductService
            var player = TestHelper.ProductService.GetAllData();

            // Select a player's ID
            var data = player.Skip(2).First().Id;

            // Assign a player's ID to the SelectedPlayerLeftId property
            pageModel.SelectedPlayerLeftId = data;

            // Assign null to the SelectedPlayerRightId property
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

            // Retrieve all player data using the ProductService
            var player = TestHelper.ProductService.GetAllData();

            // Select a player's ID
            var data = player.Skip(2).First().Id;

            // Assign null to the SelectedPlayerLeftId property
            pageModel.SelectedPlayerLeftId = null;

            // Assign a player's ID to the SelectedPlayerRightId property
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

            // Retrieve all player data using the ProductService
            var player = TestHelper.ProductService.GetAllData();

            // Select the third player's ID (zero-based index, skipping the first two)
            var data1 = player.Skip(2).First().Id;

            // Select the fourth player's ID (zero-based index, skipping the first three)
            var data2 = player.Skip(3).First().Id;

            // Assign the third player's ID to the SelectedPlayerLeftId property
            pageModel.SelectedPlayerLeftId = data1;

            // Assign the fourth player's ID to the SelectedPlayerRightId property
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

            // create selected items
            var data = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Player One" },
                new SelectListItem { Value = "2", Text = "Player Two" }
            };

            // Act
            pageModel.PlayerDropDownItems = data;

            // Result of the dropdown items
            var result = pageModel.PlayerDropDownItems;

            // Assert
            Assert.That(result, Is.EqualTo(data), "PlayerDropDownItems should return the assigned list.");
        }

        #endregion OnGet

        #region GenerateChart

        /// <summary>
        /// GenerateChart_NoSelectedPlayerLeft_Should_ReturnWithoutCreatingChart.
        /// Ensures the method exits early when SelectedPlayerLeft is null.
        /// </summary>
        [Test]
        public void GenerateChart_Valid_No_Selected_Player_Left_Should_Return_Without_Creating_Chart()
        {
            // Arrange

            // Retrieve all player data using the ProductService
            var player = TestHelper.ProductService.GetAllData();

            // Select a player
            var data = player.Skip(2).First();

            // Assign a player to the SelectedPlayerLeft property
            pageModel.SelectedPlayerLeft = null;

            // Assign null to the SelectedPlayerRight property
            pageModel.SelectedPlayerRight = data;

            pageModel.comparisonChartBase64 = null;


            // Act
            pageModel.GenerateChart();

            // Assert
            // Assert no exceptions thrown and no chart saved
            Assert.That(pageModel.comparisonChartBase64, Is.Null);
        }

        /// <summary>
        /// GenerateChart_NoSelectedPlayerLeft_Should_ReturnWithoutCreatingChart.
        /// Ensures the method exits early when SelectedPlayerLeft is null.
        /// </summary>
        [Test]
        public void GenerateChart_Valid_No_Selected_Player_Right_Should_Return_Without_Creating_Chart()
        {
            // Arrange

            // Retrieve all player data using the ProductService
            var player = TestHelper.ProductService.GetAllData();

            // Select a player
            var data = player.Skip(2).First();

            // Assign null to the SelectedPlayerLeft property
            pageModel.SelectedPlayerLeft = data;

            // Assign a player to the SelectedPlayerRight property
            pageModel.SelectedPlayerRight = null;
            pageModel.comparisonChartBase64 = null;


            // Act
            pageModel.GenerateChart();

            // Assert
            // Assert no exceptions thrown and no chart saved
            Assert.That(pageModel.comparisonChartBase64, Is.Null);
        }

        /// <summary>
        /// GenerateChart_ValidPlayers_Should_CreateChartSuccessfully.
        /// Verifies that the chart is generated successfully with valid players.
        /// </summary>
        [Test]
        public void GenerateChart_Valid_Players_Should_CreateChartSuccessfully()
        {
            // Arrange

            // Retrieve all player data using the ProductService
            var player = TestHelper.ProductService.GetAllData();

            // Select a player
            var data1 = player.Skip(2).First();

            // Select another player
            var data2 = player.Skip(3).First();

            // Assign null to the SelectedPlayerLeft property
            pageModel.SelectedPlayerLeft = data1;

            // Assign a player to the SelectedPlayerRight property
            pageModel.SelectedPlayerRight = data2;
            pageModel.comparisonChartBase64 = null;



            // Act
            pageModel.GenerateChart();

            // Assert
            Assert.That(pageModel.comparisonChartBase64, Is.Not.Null);
        }

        #endregion GenerateChart
    }
}
