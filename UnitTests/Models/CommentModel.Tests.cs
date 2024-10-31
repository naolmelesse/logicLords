using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{
    public class CommentModelTests
    {
        #region TestSetup

        public static CommentModel commentModel;

        [SetUp]
        public void TestInitialize()
        {
            // Initialize the CommentModel before each test
            commentModel = new CommentModel();
        }

        #endregion TestSetup

        #region Property Tests

        [Test]
        public void Id_Should_Be_Initialized_With_UniqueValue()
        {

            // Arrange


            // Act
            var result = commentModel.Id;

            // Assert
            Assert.That(result, Is.Not.Null, "Id should be initialized with a non-null value.");
            Assert.That(System.Guid.TryParse(result, out _), Is.True, "Id should be a valid GUID.");
        }

        [Test]
        public void Comment_Property_Should_Be_Assignable_And_Retrievable()
        {
            // Arrange
            var data = "This is a test comment.";

            // Act
            commentModel.Comment = data;
            var result = commentModel.Comment;

            // Assert
            Assert.That(result, Is.EqualTo(data), "The Comment property should return the assigned value.");
        }

        [Test]
        public void Id_Set_ValidValue_Should_Return_CorrectId()
        {
            // Arrange
            var newId = "12345-abcd-67890-efgh-12345";

            // Act
            commentModel.Id = newId;
            var result = commentModel.Id;

            // Assert
            Assert.That(result, Is.EqualTo(newId), "The Id property should return the newly assigned value.");
        }

        #endregion Property Tests
    }

}
