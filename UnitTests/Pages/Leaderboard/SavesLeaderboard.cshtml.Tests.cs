using ContosoCrafts.WebSite.Pages.Leaderboard;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Leaderboard
{

    /// <summary>
    /// Unit tests for the Saves Leaderboard page functionality.
    /// </summary>
    class SavesLeaderboardTest
    {
        #region TestSetup

        // The SavesLeaderboardModel instance being tested
        public static SavesLeaderboardModel pageModel;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a mock logger for the SavessLeaderboardModel
            var MockLoggerDirect = Mock.Of<ILogger<SavesLeaderboardModel>>();

            // Initialize the SavesLeaderboardModel with the mock logger and test product service
            pageModel = new SavesLeaderboardModel(MockLoggerDirect, TestHelper.ProductService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Verifies that the PlayerList is populated and sorted in descending order of Saves 
        /// after the OnGet method is called.
        /// </summary>
        [Test]
        public void OnGet_Valid_Given_Player_List_Should_Populate_Player_List_In_Descending_Order_Of_Saves()
        {
            // Arrange
            var players = TestHelper.ProductService.GetAllData();
            pageModel.PlayerList = players;

            // Act
            // Invoke the OnGet method
            pageModel.OnGet();


            // Assert
            //  Verify that the PlayerList is sorted in descending order of Saves
            var orderedList = pageModel.PlayerList.ToList();
            for (int i = 1; i < orderedList.Count; i++)
            {
                Assert.That(orderedList[i - 1].Saves >= orderedList[i].Saves, Is.True);
            }
        }

        #endregion OnGet
    }
}