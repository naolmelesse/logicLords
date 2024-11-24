namespace ContosoCrafts.WebSite.Models
{
    
    
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

    public static class PlayerTypeEnumExtensions
    {
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


                // Default, Unknown
                _ => "",
            };
        }
    }
}