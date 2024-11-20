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

        //Player's Age
        [Range(0, 125, ErrorMessage = "Age must be between 0 and 125")]
        public int Age { get; set; }

        // The soccer club the player is contracted to
        [Required(ErrorMessage = "Current Club is required")]
        [StringLength(100, ErrorMessage = "Club cannot be longer than 100 characters.")]
        public string Club { get; set; }

        // Player's height in both metric and imperial
        [Required(ErrorMessage = "Height is required")]
        [StringLength(100, ErrorMessage = "Height cannot be longer than 100 characters.")]
        public string Height { get; set; }

        // Player's height in centimeters
        public int HeightInCMs { get; set; }

        // Player's usual position on the field
        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot be longer than 50 characters.")]
        public string Position { get; set; }

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

    }
}