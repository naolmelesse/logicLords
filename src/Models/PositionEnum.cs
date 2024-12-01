namespace ContosoCrafts.WebSite.Models
{

    /// <summary>
    /// Represents various positions a player can have in a game or team.
    /// </summary>
    public enum PositionEnum
    {
        Undefined = 0,
        Striker = 1,
        Forward = 5,
        Winger = 25,
        Midfielder = 130,
        Defender = 55,
        Goalkeeper = 60,
    }

    /// <summary>
    /// Extension methods for the <see cref="PositionEnum"/> enumeration.
    /// </summary>
    public static class PlayerTypeEnumExtensions
    {

        /// <summary>
        /// Gets the display name for a given <see cref="PositionEnum"/> value.
        /// </summary>
        /// <param name="data">The <see cref="PositionEnum"/> value.</param>
        /// <returns>A string representing the display name of the position, or an empty string for undefined or unknown values.</returns>
        public static string DisplayName(this PositionEnum data)
        {
            return data switch
            {
                PositionEnum.Striker => "Striker",
                PositionEnum.Forward => "Forward",
                PositionEnum.Winger => "Winger",
                PositionEnum.Midfielder => "Midfielder",
                PositionEnum.Defender => "Defender",
                PositionEnum.Goalkeeper => "Goalkeeper",


                // Default case for unknown or undefined values
                _ => "",
            };
        }
    }
}