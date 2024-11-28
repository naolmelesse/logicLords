using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// The UpdateModel class handles the update functionality for a player.
    /// It retrieves a product by its Id and updates it using the JsonFileProductService.
    /// </summary>
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;

        /// <summary>
        /// The Id of the player to be updated. 
        /// This property supports GET requests.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        // The player to be updated.
        [BindProperty]
        public ProductModel Player { get; set; }

        // The service used to handle player data operations.
        public JsonFileProductService PlayerService { get; }

        /// <summary>
        /// Constructor for the UpdateModel class.
        /// Initializes the PlayerService used to manage player data.
        /// </summary>
        /// <param name="playerService">The service for handling player data operations.</param>
        /// <param name="logger">Logger for logging errors and information.</param>
        public UpdateModel(JsonFileProductService playerService, ILogger<UpdateModel> logger)
        {
            PlayerService = playerService;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to retrieve a player by its Id.
        /// Populates the Player attribute if a matching player is found.
        /// </summary>
        public IActionResult OnGet()
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    throw new Exception("Player ID is missing.");
                }

                Player = PlayerService.GetAllData().FirstOrDefault(p => p.Id == Id);

                if (Player == null)
                {
                    throw new Exception($"Player with ID: '{Id}' not found.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred on the Update page during OnGet.");
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToPage("/Error");
            }
        }

        /// <summary>
        /// Handles POST requests to update a product.
        /// Checks if Model's state is valid before using PlayerService.
        /// </summary>
        /// <returns>
        /// Redirects to Index Page if Model state valid and update successful; 
        /// otherwise, redirects to the Error page in case of an exception.
        /// </returns>
        public IActionResult OnPost()
        {
            try
            {
                // Check if the model's state is invalid
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (Player == null)
                {
                    throw new Exception("Player data is missing.");
                }

                PlayerService.UpdateData(Player);

                return RedirectToPage("/Product/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred on the Update page during OnPost.");
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToPage("/Error");
            }
        }
    }
}
