using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// PageModel for handling the Delete functionality in the Player section.
    /// </summary>
    public class DeleteModel : PageModel
    {
        // Initializes a new instance of the <see cref="DeleteModel"/> class.
        private readonly JsonFileProductService _playerService;


        /// <summary>
        /// Initializes a new instance of the DeleteModel class.
        /// </summary>
        /// <param name="playerService">The player service for managing player data operations.</param>
        public DeleteModel(JsonFileProductService playerService)
        {
            _playerService = playerService;
        }


        // Gets or sets the Player to be deleted.
        [BindProperty]
        public ProductModel Player { get; set; }

        /// <summary>
        /// Handles GET requests to load and confirm the player for deletion.
        /// </summary>
        /// <param name="id">The unique identifier of the player to delete.</param>
        /// <returns>The page displaying the player if found; redirects to an error page if not found.</returns>

        public IActionResult OnGet(string id)
        {
            // Load the product to confirm deletion
            Player = _playerService.GetAllData().FirstOrDefault(p => p.Id == id);
            if (Player == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        /// <summary>
        /// Handles POST requests to delete the specified player.
        /// </summary>
        /// <param name="id">The unique identifier of the player to delete.</param>
        /// <returns>Redirects to the index page if deletion is successful; redirects to an error page if deletion fails.</returns>
        public IActionResult OnPost(string id)
        {
            // Call DeleteData to delete the player
            var deletedPlayer = _playerService.DeleteData(id);

            // Check if the player to be deleted doesn't exist
            if (deletedPlayer == null)
            {
                return RedirectToPage("/Error"); 
            }

            return RedirectToPage("Index"); 
        }
    }
}
