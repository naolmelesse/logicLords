using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    ///   Page Model for the Privacy HTML page
    /// </summary>
    public class PrivacyModel : PageModel
    {
        // Logger for this model
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the GET request for the Privacy page
        /// </summary>
        public void OnGet()
        {
        }
    }
}
