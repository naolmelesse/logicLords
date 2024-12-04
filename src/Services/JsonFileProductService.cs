using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ContosoCrafts.WebSite.Services
{


    /// <summary>
    /// Service to manage product data stored in a JSON file.
    /// Provides methods to perform CRUD operations and handle product ratings.
    /// </summary>
    public class JsonFileProductService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFileProductService"/> class.
        /// </summary>
        /// <param name="webHostEnvironment">Provides information about the web hosting environment.</param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }


        /// <summary>
        /// Gets the web host environment used by the service.
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }


        /// <summary>
        /// Gets the full path of the JSON file used to store product data.
        /// </summary>
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }


        /// <summary>
        /// Retrieves all product data from the JSON file.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ProductModel}"/> of all product records.</returns>
        public virtual IEnumerable<ProductModel> GetAllData()
        {
            using(var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        /// <summary>
        /// Add Rating
        /// 
        /// Take in the product ID and the rating
        /// If the rating does not exist, add it
        /// Save the update
        /// </summary>
        /// <param name="productId">The ID of the product to rate.</param>
        /// <param name="rating">The rating value to add (must be between 0 and 5).</param>
        /// <returns>True if the rating was successfully added; otherwise, false.</returns>
        public bool AddRating(string productId, int rating)
        {
            // If the ProductID is invalid, return
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }

            var products = GetAllData();

            // Look up the product, if it does not exist, return
            var data = products.FirstOrDefault(x => x.Id.Equals(productId));
            if (data == null)
            {
                return false;
            }

            // Check Rating for boundaries, do not allow ratings below 0
            if (rating < 0)
            {
                return false;
            }

            // Check Rating for boundaries, do not allow ratings above 5
            if (rating > 5)
            {
                return false;
            }

            // Check to see if the rating exist, if there are none, then create the array
            if(data.Ratings == null)
            {
                data.Ratings = new int[] { };
            }

            // Add the Rating to the Array
            var ratings = data.Ratings.ToList();
            ratings.Add(rating);
            data.Ratings = ratings.ToArray();

            // Save the data back to the data store
            SaveData(products);

            return true;
        }

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data">The product data containing updated values.</param>
        /// <returns>The updated product model, or null if the product was not found.</returns>
        public ProductModel UpdateData(ProductModel data)
        {
            //Get all products
            var products = GetAllData();

            // Check if data is null
            if (data == null)
            {
                return null;
            }

            //Find the data from the products
            var productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));

            //Check if the product was found
            if (productData == null)
            {
                return null;
            }

            // Update the data to the new passed in values
            productData.Title = data.Title;
            productData.Club = data.Club;
            productData.Minutesplayed = data.Minutesplayed;
            productData.Placeofbirth = data.Placeofbirth;
            productData.Assists = data.Assists;
            productData.Goals = data.Goals;
            productData.Appearances = data.Appearances;
            productData.Position = data.Position;
            productData.HeightInCMs = data.HeightInCMs;
            productData.Image = data.Image;
            productData.Dateofbirth = data.Dateofbirth;


            // Save the updated product data.
            SaveData(products);

            return productData;
        }

        /// <summary>
        /// Save All products data to storage
        /// </summary>
        private void SaveData(IEnumerable<ProductModel> products)
        {

            // Open the JSON file for writing.
            using (var outputStream = File.Create(JsonFileName))
            {
                // Serialize the product data and write to the JSON file.
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
            }
        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns>The newly created product model.</returns>
        public ProductModel CreateData()
        {
            // Create a new product with default values.
            var data = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Image = "",
            };

            // Get the current set, and append the new record to it because IEnumerable does not have Add
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            SaveData(dataSet);

            return data;
        }

        /// <summary>
        /// Creates a new player entry in the database with a unique ID.
        /// </summary>
        /// <param name="newPlayer">The product model representing the new player.</param>
        /// <returns>The newly created player with a unique ID, or null if input is invalid.</returns>
        public ProductModel CreateData(ProductModel newPlayer)
        {
            // Validate input: Ensure the new player is not null
            if (newPlayer == null)
            {
                return null;
            }

            // Retrieve all existing players and extract their IDs
            var playersInDB = GetAllData();

            // Set of existing player IDs
            var existingIds = new HashSet<string>(playersInDB.Select(player => player.Id));

            // Base ID derived from the player's title
            var baseId = newPlayer.Title.Replace(' ', '-');

            // Variable to hold the new unique ID
            var newId = baseId;
            
            // Counter for generating unique IDs if there's a conflict
            var counter = 1;  // Start counter at 1 (not 0)

            while (existingIds.Contains(newId))
            {
                newId = baseId + counter++;
            }

            newPlayer.Id = newId;

            playersInDB = playersInDB.Append(newPlayer).ToList();

            SaveData(playersInDB);

            return newPlayer;
        }

        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>The data set after the delete action</returns>
        public ProductModel DeleteData(string id)
        {
            // Get the current set, and append the new record to it
            var dataSet = GetAllData();
            
            // Find the player matching the provided ID
            var data = dataSet.FirstOrDefault(m => m.Id.Equals(id));

            // Filter out the player with the given ID to create a new dataset
            var newDataSet = GetAllData().Where(m => m.Id.Equals(id) == false);
            
            // Save update dataset
            SaveData(newDataSet);

            return data;
        }
        
    }
}