using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class RegisterModel : PageModel
{
    private readonly MiniTwitContext _miniTwitContext;
    private readonly IUserRepository _userRepository;

    [BindProperty] public string Email { get; set; } = string.Empty;

    [BindProperty] public string Username { get; set; } = string.Empty;

    [BindProperty] public string Password { get; set; } = string.Empty;

    [BindProperty] public string Password2 { get; set; } = string.Empty;
    
    public string Error { get; set; } = String.Empty;
    public RegisterModel(
        MiniTwitContext miniTwitContext, 
        UserRepository userRepository)
    {
        _miniTwitContext = miniTwitContext;
        _userRepository = userRepository;
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
        else if (_userRepository.GetUserByUsername(Username) != null)
            Error = "The username is already taken";
        else
        {
            var hasher = new PasswordHasher<string>();
            var hash = hasher.HashPassword(Username, Password);
            _userRepository.CreateUser(Username, Email, hash);
            
            TempData["FlashMessage"] = "You were successfully registered and can login now";
            return RedirectToPage("/Login");
        }
        return Page();
    }
}