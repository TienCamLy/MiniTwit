using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        // Clear the existing external cookie
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        
        TempData["FlashMessage"] = "You were logged out";
        return RedirectToPage("/PublicTimeline");
    }
}