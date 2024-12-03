using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
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

        /// <summary>
        /// The Id of the player to be deleted. 
        /// This property supports GET requests.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        // Gets or sets the Player to be deleted.
        [BindProperty]
        public ProductModel Player { get; set; }

        /// <summary>
        /// Handles GET requests to load and confirm the player for deletion.
        /// </summary>
        /// <param name="id">The unique identifier of the player to delete.</param>
        /// <returns>The page displaying the player if found; redirects to an error page if not found.</returns>

        public IActionResult OnGet()
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    throw new Exception("Player ID is missing.");
                }

                // Load the product to confirm deletion
                Player = _playerService.GetAllData().FirstOrDefault(p => p.Id == Id);

                // Check if the player with the given ID was not found
                if (Player == null)
                {
                    throw new Exception($"Player with ID: '{Id}' not found.");
                    //return RedirectToPage("/Error");
                }

                return Page();
            }
            catch
            {
                // Redirect to error page in case of any exceptions
                return RedirectToPage("/Error");
            }
        }

        /// <summary>
        /// Handles POST requests to delete the specified player.
        /// </summary>
        /// <param name="id">The unique identifier of the player to delete.</param>
        /// <returns>Redirects to the index page if deletion is successful; redirects to an error page if deletion fails.</returns>
        public IActionResult OnPost()
        {
            try
            {
                // Call DeleteData to delete the player
                var deletedPlayer = _playerService.DeleteData(Id);

                // Check if the player to be deleted doesn't exist
                if (deletedPlayer == null)
                {
                    throw new Exception("Player data is missing.");
                    //return RedirectToPage("/Error");
                }

                return RedirectToPage("/Product/Index");
            }
            catch
            {
                // Redirect to error page in case of any exceptions
                return RedirectToPage("/Error");
            }
        }
    }
}