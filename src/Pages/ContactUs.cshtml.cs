using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages
{
    public class ContactUsModel : PageModel
    {
        public string PhoneNumber { get; set; } = "(123) 456-7890";
        public string Email { get; set; } = "info@dribblekings.com";
        public string AddressLine1 { get; set; } = "Seattle University";
        public string City { get; set; } = "Seattle";
        public string State { get; set; } = "WA";
        public void OnGet()
        {
        }
    }
}
