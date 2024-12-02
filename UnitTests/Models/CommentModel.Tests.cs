using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{

    /// <summary>
    /// Unit tests for the <see cref="CommentModel"/> class.
    /// This test class validates the properties of the CommentModel.
    /// </summary>
    public class CommentModelTests
    {
        #region TestSetup

        // A shared instance of <see cref="CommentModel"/> for testing.
        public static CommentModel commentModel;

        /// <summary>
        /// Initializes the test setup.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Initialize the CommentModel before each test
            commentModel = new CommentModel();
        }

        #endregion TestSetup

        #region Property Tests

        /// <summary>
        /// Test to verify that the Id property is initialized with a unique non-null value.
        /// The Id should be a valid GUID.
        /// </summary>
        [Test]
        public void Id_Should_Be_Initialized_With_UniqueValue()
        {

            // Arrange


            // Act

            // Get the Id
            var result = commentModel.Id;

            // Assert
            Assert.That(result, Is.Not.Null, "Id should be initialized with a non-null value.");
            Assert.That(System.Guid.TryParse(result, out _), Is.True, "Id should be a valid GUID.");
        }

        /// <summary>
        /// Test to verify that the Id property is initialized with a unique non-null value.
        /// The Id should be a valid GUID.
        /// </summary>
        [Test]
        public void Comment_Property_Should_Be_Assignable_And_Retrievable()
        {
            // Arrange
            var data = "This is a test comment.";

            // Act
            commentModel.Comment = data;

            //Get the Comment
            var result = commentModel.Comment;

            // Assert
            Assert.That(result, Is.EqualTo(data), "The Comment property should return the assigned value.");
        }

        /// <summary>
        /// Test to verify that the Id property can be set to a valid value and retrieved correctly.
        /// </summary>
        [Test]
        public void Id_Set_ValidValue_Should_Return_CorrectId()
        {
            // Arrange
            var newId = "12345-abcd-67890-efgh-12345";

            // Act
            commentModel.Id = newId;

            //Get the Id
            var result = commentModel.Id;

            // Assert
            Assert.That(result, Is.EqualTo(newId), "The Id property should return the newly assigned value.");
        }

        #endregion Property Tests
    }

}