using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class RegisterModel : PageModel
{
    private readonly IDBContext _dbcontext;
    
    [BindProperty]
    public string Email { get; set; }
    
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }
    
    [BindProperty] 
    public string Password2 { get; set; }
    
    public string Error { get; set; }
    public RegisterModel(IDBContext dbcontext)
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
        else if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
            Error = "You have to enter a valid email address";
        else if (string.IsNullOrEmpty(Password))
            Error = "You have to enter a password";
        else if (Password != Password2)
            Error = "The two passwords do not match";
        else if (_dbcontext.GetUserById(Username) != null)
            Error = "The username is already taken";
        else
        {
            var hasher = new PasswordHasher<string>();
            var hash = hasher.HashPassword(null, Password);
            _dbcontext.CreateUser(Username, Email, hash);
            return RedirectToPage("/Login");
        }
        return Page();
    }
}