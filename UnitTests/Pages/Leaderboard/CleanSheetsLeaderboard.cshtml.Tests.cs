using ContosoCrafts.WebSite.Pages.Leaderboard;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Leaderboard
{
    /// <summary>
    /// Class for unit tests of clean Sheets Leaderboard feature
    /// </summary>
    class CleanSheetsLeaderboardTests
    {
        #region TestSetup

        // The Assists Leaderboard Page Model instance being tested
        public static CleanSheetsLeaderboardModel pageModel;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a mock logger for the AssistsLeaderboardModel
            var MockLoggerDirect = Mock.Of<ILogger<CleanSheetsLeaderboardModel>>();

            // Initialize the AssistsLeaderboardModel with the mock logger and test product service
            pageModel = new CleanSheetsLeaderboardModel(MockLoggerDirect, TestHelper.ProductService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test case for the OnGet method.
        /// This test verifies that when the OnGet method is called, the Player list is sorted by clean sheets.
        /// </summary>
        [Test]
        public void OnGet_Valid_Given_Player_List_Should_Populate_Player_List_In_Descending_Order_Of_CleanSheets()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            pageModel.PlayerList = players;

            // Act
            // Call the OnGet method of the page model
            pageModel.OnGet();


            // Assert
            // Player list ordered by clean sheets
            var orderedList = pageModel.PlayerList.ToList();
            for (int i = 1; i < orderedList.Count; i++)
            {
                Assert.That(orderedList[i - 1].CleanSheets >= orderedList[i].CleanSheets, Is.True);
            }
        }

        #endregion OnGet
    }
}