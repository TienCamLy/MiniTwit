using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_pages.Structs;

namespace razor_pages.Pages;

public class LoginModel : PageModel
{

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Error { get; set; }

    private readonly IDBContext _dbcontext;
    public LoginModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    
    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Username))
            Error = "You have to enter a username";
        else if (string.IsNullOrEmpty(Password))
            Error = "You have to enter a password";
        else
        {
            Console.WriteLine($"Logging in... {Username} {Password}");
            var user = _dbcontext.Login(Username, Password);
            if (user != null)
            {
                // TODO: set user in session
                return RedirectToPage("/Timeline");
            }
            else
            {
                Error = "Invalid username or password";
            }
        }
        return Page();
    }
}