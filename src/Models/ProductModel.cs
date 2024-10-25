using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Class Representing the player model
    /// </summary>
    public class ProductModel
    {
        // Unique Identifier for a player
        public string Id { get; set; }        

        // Full URL to the player's image
        [JsonPropertyName("img")]
        public string Image { get; set; }

        // Player's place of birth
        [JsonPropertyName("Place of birth")]
        public string Placeofbirth { get; set; }

        //Player's Date of Birth
        [JsonPropertyName("Date of birth")]
        public string Dateofbirth { get; set; }

        // The soccer club the player is contracted to
        public string Club { get; set; }

        // Player's height in both metric and imperial
        public string Height { get; set; }

        // Player's usual position on the field
        public string Position { get; set; }

        // Number of goals scored by the player
        public string Goals { get; set; }

        // Number of assists made by the player
        public string Assists { get; set; }

        // The number of appearances a player has made (starts and subs)
        public string Appearances { get; set; }

        // Total minutes the player has made
        [JsonPropertyName("Minutes played")]
        public string Minutesplayed { get; set; }
       
        // The name of the player
        public string Title { get; set; }       

        // Ratings recieved for the player
        public int[] Ratings { get; set; }   
        
        // Override ToString to serialize the object to a JSON string
        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);

    }
}