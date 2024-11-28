using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace UnitTests.Models
{

    /// <summary>
    /// Unit tests for the ProductModel class.
    /// </summary>
    public class ProductModelTests
    {
        #region TestSetup

        // Static instance of ProductModel for testing.
        public static ProductModel productModel;

        /// <summary>
        /// Initializes the ProductModel instance before testing.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Initialize the ProductModel with sample data
            productModel = new ProductModel
            {
                Id = "AitanaBonmati",
                Title = "Aitana Bonmati",
            };
        }

        #endregion TestSetup

        #region ToString Method Tests

        /// <summary>
        /// Test case for a valid ProductModel instance, expecting ToString to return a JSON string representation of the model.
        /// </summary>
        [Test]
        public void ToString_Valid_ProductModel_Should_Return_JsonString()
        {
            // Arrange

            // Expected JSON string from serializing the productModel instance
            var expectedJson = JsonSerializer.Serialize(productModel);

            // Act

            // Call ToString to get the JSON string representation
            var result = productModel.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedJson), "The ToString method should return a JSON representation of the product model.");
        }

        /// <summary>
        /// Test case for an empty ProductModel instance, expecting ToString to return a JSON string representation of an empty model.
        /// </summary>
        [Test]
        public void ToString_Empty_ProductModel_Should_Return_JsonString()
        {
            // Arrange

            // Initialize an empty ProductModel instance
            var emptyProductModel = new ProductModel();

            // Expected JSON string from serializing the emptyProductModel instance
            var expectedJson = JsonSerializer.Serialize(emptyProductModel);

            // Act

            // Call ToString to get the JSON string representation
            var result = emptyProductModel.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedJson), "The ToString method should return a JSON representation of an empty product model.");
        }

        #endregion ToString Method Tests

        #region ValidatePosition Method Tests

        /// <summary>
        /// Test that ValidatePosition returns a validation error when the position is Undefined.
        /// </summary>
        [Test]
        public void ValidatePosition_UndefinedPosition_ShouldReturnValidationError()
        {
            // Arrange
            var position = PositionEnum.Undefined;

            // Act
            var result = ProductModel.ValidatePosition(position);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Position cannot be Undefined."));
        }

        /// <summary>
        /// Test that the ValidatePosition method returns success for all valid positions.
        /// </summary>
        [Test]
        public void ValidatePosition_ValidPositions_ShouldReturnSuccess()
        {
            // Arrange
            var validPositions = new[]
            {
                PositionEnum.Striker,
                PositionEnum.Forward,
                PositionEnum.Winger,
                PositionEnum.Midfielder,
                PositionEnum.Defender,
                PositionEnum.Goalkeeper
            };

            foreach (var position in validPositions)
            {
                // Act
                var result = ProductModel.ValidatePosition(position);

                // Assert
                Assert.That(result, Is.EqualTo(ValidationResult.Success), $"Position {position} failed validation unexpectedly.");
            }
        }

        #endregion ValidatePosition Method Tests
    }
}
