using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Numerics;

namespace ContosoCrafts.WebSite.Pages.Shared
{
    /// <summary>
    /// Page model for the LeaderboardSnippet partial
    /// </summary>
    public class LeaderboardSnippetModel : PageModel
    {
       
        // Title of the leaderboard
        public string Title { get; set; }

        // Collection of players to display in the leaderboard
        public IEnumerable<ProductModel> Players { get; set; }

        // URL to navigate to the "Show More" page
        public string ShowMoreUrl { get; set; }

        // Statistic to display (e.g., Goals, Assists)
        public string DisplayStat { get; set; }

        /// <summary>
        /// Retrieves the value of a specified property from a player object.
        /// </summary>
        /// <param name="player">The player object to retrieve the property value from.</param>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <returns>The value of the specified property, or null if the property does not exist.</returns>
        public object GetPropertyValue(ProductModel player, string propertyName)
        {

            // Get the property information for the specified property name
            var property = player.GetType().GetProperty(propertyName);

            if (property == null)
            {
                return null;
            }
            return property.GetValue(player, null);
        }
    }
}