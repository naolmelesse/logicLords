using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// Page Model for the Error Page
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // Gets or sets the request ID associated with the current HTTP request.
        public string RequestId { get; set; }

        // Gets a value indicating whether the request ID should be shown.
        // Returns true if the RequestId is not null or empty.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Logger instance for capturing log details related to errors.
        private readonly ILogger<ErrorModel> _logger;

        /// <summary>
        /// Initializes a new instance of the ErrorModel class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        // Stores the exception message to display on the Error page.
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Handles GET requests to the error page.
        /// Captures and sets the RequestId for the current HTTP context.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // Retrieve the error message from TempData
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ErrorMessage = TempData["ErrorMessage"] as string;
            }
        }
    }
}