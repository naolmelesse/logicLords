using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Models
{
    public class ProductModel
    {
        public string Id { get; set; }        

        [JsonPropertyName("img")]
        public string Image { get; set; }

        [JsonPropertyName("Place of birth")]
        public string Placeofbirth { get; set; }

        [JsonPropertyName("Date of birth")]
        public string Dateofbirth { get; set; }

        public string Club { get; set; }

        public string Height { get; set; }

        public string Position { get; set; }

        public string Goals { get; set; }
        public string Assists { get; set; }
        public string Appearances { get; set; }

        [JsonPropertyName("Minutes played")]
        public string Minutesplayed { get; set; }
       
        public string Title { get; set; }       

        public int[] Ratings { get; set; }   
        

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);


    }
}