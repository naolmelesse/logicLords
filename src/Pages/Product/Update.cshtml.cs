using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// The UpdateModel class handles the update functionality for a player.
    /// It retrieves a product by its Id and updates it using the JsonFileProductService.
    /// </summary>
    public class UpdateModel : PageModel
    {

        /// <summary>
        /// The Id of the player to be updated. 
        /// This property supports GET requests.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        //The ID of the player to be updated. 
        [BindProperty]
        public ProductModel Player { get; set; }

        // The service used to handle player data operations.
        public JsonFileProductService PlayerService { get; }

        /// <summary>
        /// Constructor for the UpdateModel class.
        /// Initializes the PlayerService used to manage player data.
        /// </summary>
        /// <param name="playerService">The service for handling player data operations.</param>
        public UpdateModel(JsonFileProductService playerService)
        {
            PlayerService = playerService;
        }

        /// <summary>
        /// Handles GET requests to retrieve a player by its Id.
        /// Populates the Player property if a matching product is found.
        /// </summary>
        public void OnGet()
        {
            Player = PlayerService.GetAllData().FirstOrDefault(p => p.Id == Id);
        }

        /// <summary>
        /// Handles POST requests to update a product.
        /// Checks if Model's state is valid before using PlayerService.
        /// </summary>
        /// <returns>
        /// Redirects to Index Page if Model state valid and update successful; 
        /// otherwise, returns the current page.
        /// </returns>
        public IActionResult OnPost()
        {
            //Check if the model's state is invalid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PlayerService.UpdateData(Player);

            return RedirectToPage("/Index");
        }
    }
}
