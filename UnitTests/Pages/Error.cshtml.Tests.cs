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
        /// Test case for a valid activity with an active ID which expects the RequestId to match the activity ID.
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
        /// Test case for an null which expects the RequestId to be set to "trace" and ShowRequestId to be true.
        /// </summary>
        [Test]
        public void OnGet_Invalid_Activity_Null_Should_Return_TraceIdentifier()
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

        /// <summary>
        /// Test case when TempData contains ErrorMessage and expects it to be set correctly.
        /// </summary>
        [Test]
        public void OnGet_Invalid_TempData_Has_ErrorMessage_Should_Set_ErrorMessage()
        {
            // Arrange
            var mockLogger = Mock.Of<ILogger<ErrorModel>>();
            var pageModel = new ErrorModel(mockLogger)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData
            };

            pageModel.TempData["ErrorMessage"] = "Test error message";

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ErrorMessage, Is.EqualTo("Test error message"));
            Assert.That(pageModel.RequestId, Is.Not.Null);
            Assert.That(pageModel.ShowRequestId, Is.True);
        }

        /// <summary>
        /// Test case when ErrorMessage is null.
        /// </summary>
        [Test]
        public void OnGet_Valid_TempData_Does_Not_Have_ErrorMessage_Should_Not_Set_ErrorMessage()
        {
            // Arrange
            var mockLogger = Mock.Of<ILogger<ErrorModel>>();
            var pageModel = new ErrorModel(mockLogger)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData
            };

            pageModel.TempData.Remove("ErrorMessage");

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ErrorMessage, Is.Null);
            Assert.That(pageModel.RequestId, Is.Not.Null);
            Assert.That(pageModel.ShowRequestId, Is.True);
        }

        /// <summary>
        /// Test case for the ShowRequestId property when RequestId is null or empty.
        /// </summary>
        [Test]
        public void ShowRequestId_Invalid_RequestId_Is_Null_Should_Return_False()
        {
            // Arrange
            var mockLogger = Mock.Of<ILogger<ErrorModel>>();
            var pageModel = new ErrorModel(mockLogger)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
                RequestId = null
            };

            // Act
            var result = pageModel.ShowRequestId;

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Test case for the constructor to ensure it initializes correctly.
        /// </summary>
        [Test]
        public void Constructor_Valid_Initialization_Should_Succeed()
        {
            // Arrange
            var mockLogger = Mock.Of<ILogger<ErrorModel>>();

            // Act
            var pageModel = new ErrorModel(mockLogger);

            // Assert
            Assert.That(pageModel, Is.Not.Null);
            Assert.That(pageModel.RequestId, Is.Null);
            Assert.That(pageModel.ErrorMessage, Is.Null);
        }

        #endregion OnGet
    }
}