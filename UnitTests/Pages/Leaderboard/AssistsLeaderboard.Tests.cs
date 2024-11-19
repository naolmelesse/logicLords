using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Pages.Leaderboard;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Pages.Leaderboard
{

    /// <summary>
    /// Class for unit tests of Assists Leaderboard feature
    /// </summary>
    class AssistsLeaderboardTests
    {
        #region TestSetup

        // The Assists Leaderboard Page Model instance being tested
        public static AssistsLeaderboardModel pageModel;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a mock logger for the AssistsLeaderboardModel
            var MockLoggerDirect = Mock.Of<ILogger<AssistsLeaderboardModel>>();

            // Initialize the AssistsLeaderboardModel with the mock logger and test product service
            pageModel = new AssistsLeaderboardModel(MockLoggerDirect, TestHelper.ProductService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test case for the OnGet method.
        /// This test verifies that when the OnGet method is called, the Player list is sorted by assists.
        /// </summary>
        [Test]
        public void OnGet_Valid_Given_Player_List_Should_Populate_Player_List_In_Descending_Order_Of_Assists()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            pageModel.PlayerList = players;

            // Act
            // Call the OnGet method of the page model
            pageModel.OnGet();


            // Assert
            // Player list ordered by assists
            var orderedList = pageModel.PlayerList.ToList();
            for (int i = 1; i < orderedList.Count; i++)
            {
                Assert.That(orderedList[i - 1].Assists >= orderedList[i].Assists, Is.True);
            }
        }

        #endregion OnGet
    }
}