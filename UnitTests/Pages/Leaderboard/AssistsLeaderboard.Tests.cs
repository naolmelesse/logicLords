using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Pages.Leaderboard;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Pages.Leaderboard
{
    /// <summary>
    /// Class for unit tests of Compare players feature
    /// </summary>
    class AssistsLeaderboardTests
    {
        #region TestSetup

        // The Compare Player Page Model instance being tested
        public static AssistsLeaderboardModel pageModel;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a mock logger for the ComparePlayersModel
            var MockLoggerDirect = Mock.Of<ILogger<AssistsLeaderboardModel>>();

            
        }

        #endregion TestSetup
    }
}
