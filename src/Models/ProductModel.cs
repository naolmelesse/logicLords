using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Class Representing the player model
    /// </summary>
    public class ProductModel
    {

        // Unique Identifier for a player
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Id { get; set; }

        // Full URL to the player's image
        [JsonPropertyName("img")]
        public string Image { get; set; }

        // Player's place of birth
        [JsonPropertyName("Place of birth")]
        [Required(ErrorMessage = "Place of Birth is required")]
        [StringLength(100, ErrorMessage = "Place of Birth cannot be longer than 100 characters.")]
        public string Placeofbirth { get; set; }

        //Player's Date of Birth
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date of Birth")]
        [Range(typeof(DateTime), "01/01/1800", "01/01/2025", ErrorMessage = "Date of Birth has to be in the past 200 Years")]
        [JsonPropertyName("Date of birth")]
        public string Dateofbirth { get; set; }

        //Player's Age calculated from Date of Birth
        [JsonIgnore]
        public int Age
        {
            get
            {
                if (DateTime.TryParse(Dateofbirth, out DateTime dob))
                {
                    // Today date
                    var today = DateTime.Today;

                    // Age of player
                    int age = today.Year - dob.Year;

                    if (dob > today.AddYears(-age)) age--;
                    return age;
                }
                return 0;
            }
        }

        // The soccer club the player is contracted to
        [Required(ErrorMessage = "Current Club is required")]
        [StringLength(100, ErrorMessage = "Club cannot be longer than 100 characters.")]
        public string Club { get; set; }

        // Player's height in meters as a string
        [JsonIgnore] // Prevents serialization as HeightInCM will be calculated
        public string Height => (HeightInCMs / 100.0).ToString("0.00") + "m";

        // Player's height in centimeters
        [Range(0, int.MaxValue, ErrorMessage = "Height must be a non-negative number.")]
        public int HeightInCMs { get; set; }

        // Player's position on the field
        [Required(ErrorMessage = "Position is required.")]
        [EnumDataType(typeof(PositionEnum), ErrorMessage = "Invalid position selected.")]
        [CustomValidation(typeof(ProductModel), nameof(ValidatePosition))]
        public PositionEnum Position { get; set; } = PositionEnum.Undefined;

        // Number of goals scored by the player
        [Range(0, int.MaxValue, ErrorMessage = "Goals must be a non-negative number.")]
        public int Goals { get; set; }

        // Number of assists made by the player
        [Range(0, int.MaxValue, ErrorMessage = "Assists must be a non-negative number.")]
        public int Assists { get; set; }

        // The number of appearances a player has made (starts and subs)
        [Range(0, int.MaxValue, ErrorMessage = "Appearances must be a non-negative number.")]
        public int Appearances { get; set; }

        // Total minutes the player has made
        [Required(ErrorMessage = "Minutes Played is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Minutes Played must be a non-negative number.")]
        [JsonPropertyName("Minutes played")]
        public int Minutesplayed { get; set; }

        // The name of the player
        [JsonPropertyName("Title")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Title { get; set; }

        // The number of games where a player doesn't concede
        [Range(0, int.MaxValue, ErrorMessage = "Clean Sheets must be a non-negative number.")]
        [JsonPropertyName("Clean sheets")]
        public int CleanSheets { get; set; }

        // Number of saves made by a goalkeeper during the game
        [Range(0, int.MaxValue, ErrorMessage = " Saves must be a non-negative number.")]
        public int Saves { get; set; }

        // Ratings recieved for the player
        public int[] Ratings { get; set; }

        /// <summary>
        /// Override ToString to serialize the object to a JSON string
        /// </summary>
        /// <returns>String representation of the data</returns>
        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);

        /// <summary>
        /// Custom validation method for ensuring player position is not undefined
        /// </summary>
        /// <param name="position"> The position selected </param>
        /// <returns>
        /// ValidationResult indicating a successful validation or error message if fail
        /// </returns>
        public static ValidationResult ValidatePosition(PositionEnum position)
        {

            if (position == PositionEnum.Undefined)
            {

                return new ValidationResult("Position cannot be Undefined.");
            }

            return ValidationResult.Success;
        }
    }
}