using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class LoginModel : PageModel
{
    public string? error { get; set; }
    public void OnGet()
    {
    }
}