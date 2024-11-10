using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Represents the model for the Contact Us page
    /// </summary>
    public class ContactUsModel : PageModel
    {
        //Gets the phone number for contact
        public string PhoneNumber { get; } = "(123) 456-7890";

        // Gets the email address for contact
        public string Email { get;} = "info@dribblekings.com";

        // Gets the first line of the address for contact
        public string AddressLine1 { get;} = "Seattle University";

        // Gets the city of the address for contact
        public string City { get;} = "Seattle";

        // Gets the state of the address for contact
        public string State { get;} = "WA";
    }
}
