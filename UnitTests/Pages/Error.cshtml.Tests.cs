using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Diagnostics;

namespace UnitTests.Pages.Error
{

    /// <summary>
    /// Unit tests for the ErrorModel class.
    /// </summary>
    public class ErrorTests
    {
        #region TestSetup

        /// <summary>
        /// Static instance of ErrorModel for testing.
        /// </summary>
        public static ErrorModel pageModel;

        /// <summary>
        ///  Initializes the ErrorModel instance before testing with a mock ILogger and test context.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Mock logger for ErrorModel
            var MockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();

            pageModel = new ErrorModel(MockLoggerDirect)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test case for a valid activity with an active ID. Expects the RequestId to match the activity ID.
        /// </summary>
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {
            // Arrange

            // Initialize and start an Activity with a specific ID
            Activity activity = new Activity("activity");
            activity.Start();
            Activity.Current = activity;

            // Act
            pageModel.OnGet();

            // Reset
            activity.Stop();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.RequestId, Is.EqualTo(activity.Id));
        }

        /// <summary>
        /// Test case for an invalid (null) activity. Expects the RequestId to be set to "trace" and ShowRequestId to be true.
        /// </summary>
        [Test]
        public void OnGet_InValid_Activity_Null_Should_Return_TraceIdentifier()
        {
            // Arrange
            Activity.Current = null;

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(pageModel.RequestId, Is.EqualTo("trace"));
            Assert.That(pageModel.ShowRequestId, Is.EqualTo(true));
        }
        #endregion OnGet
    }
}