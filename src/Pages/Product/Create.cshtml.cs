using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Class for CreateModel of create page
    /// </summary>
    public class CreateModel : PageModel
    {

        //The player to be created. 
        [BindProperty]
        public ProductModel Player { get; set; }

        // The service used to handle player data operations.
        public JsonFileProductService PlayerService { get; }

        /// <summary>
        /// Constructor for the CreateModel class.
        /// Initializes the PlayerService used to manage player data.
        /// </summary>
        /// <param name="playerService">The service for handling player data operations.</param>
        public CreateModel(JsonFileProductService playerService)
        {
            PlayerService = playerService;
        }

        /// <summary>
        /// Handles POST requests to Create a product.
        /// Checks if Model's state is valid before using PlayerService.
        /// </summary>
        /// <returns>
        /// Redirects to Index Page if Model state valid and Create successful; 
        /// otherwise, returns the current page.
        /// </returns>
        public IActionResult OnPost()
        {
            // Check if the model's state is invalid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PlayerService.CreateData(Player);

            return RedirectToPage("/Index");
        }
    }
}
