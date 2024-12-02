using ContosoCrafts.WebSite.Models;
using NUnit.Framework;

namespace UnitTests.Models
{
    /// <summary>
    /// Unit tests for PositionEnum and its DisplayName extension
    /// </summary>
    public class PositionEnumTests
    {
        /// <summary>
        /// Test that all enum values have the expected display names
        /// </summary>
        [Test]
        public void DisplayName_Should_Return_Correct_Display_Name_For_Enum_Values()
        {
            // Arrange & Act

            // For Striker position
            var strikerName = PositionEnum.Striker.DisplayName();

            // For Forward position
            var forwardName = PositionEnum.Forward.DisplayName();

            // For Winger position
            var wingerName = PositionEnum.Winger.DisplayName();

            // For Midfielder position
            var midfielderName = PositionEnum.Midfielder.DisplayName();

            // For Defender position
            var defenderName = PositionEnum.Defender.DisplayName();

            // For Goalkeeper position
            var goalkeeperName = PositionEnum.Goalkeeper.DisplayName();

            // For Undefined position
            var undefinedName = PositionEnum.Undefined.DisplayName();

            // Assert

            // Verify that the display names match expected values
            Assert.That(strikerName, Is.EqualTo("Striker"));
            Assert.That(forwardName, Is.EqualTo("Forward"));
            Assert.That(wingerName, Is.EqualTo("Winger"));
            Assert.That(midfielderName, Is.EqualTo("Midfielder"));
            Assert.That(defenderName, Is.EqualTo("Defender"));
            Assert.That(goalkeeperName, Is.EqualTo("Goalkeeper"));
            Assert.That(undefinedName, Is.EqualTo(""));
        }

        /// <summary>
        /// Test that the default case returns an empty string for unknown values
        /// </summary>
        [Test]
        public void DisplayName_Should_Return_Empty_String_For_Unknown_Enum_Value()
        {
            // Arrange

            // Create an unknown enum value with an invalid value
            var unknownEnumValue = (PositionEnum)(-1);

            // Act

            // Get the display name for the unknown enum value
            var result = unknownEnumValue.DisplayName();

            // Assert

            // Verify that the display name is an empty string
            Assert.That(result, Is.EqualTo(""));
        }

        /// <summary>
        /// Test that the PositionEnum contains expected values
        /// </summary>
        [Test]
        public void PositionEnum_Should_Contain_Expected_Values()
        {
            // Assert

            // Verify that each enum value matches the expected integer value
            Assert.That((int)PositionEnum.Striker, Is.EqualTo(1));
            Assert.That((int)PositionEnum.Forward, Is.EqualTo(5));
            Assert.That((int)PositionEnum.Winger, Is.EqualTo(25));
            Assert.That((int)PositionEnum.Midfielder, Is.EqualTo(130));
            Assert.That((int)PositionEnum.Defender, Is.EqualTo(55));
            Assert.That((int)PositionEnum.Goalkeeper, Is.EqualTo(60));
            Assert.That((int)PositionEnum.Undefined, Is.EqualTo(0));
        }
    }
}