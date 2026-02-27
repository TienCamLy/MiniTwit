using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;

namespace Web.Pages;

public class LoginModel : PageModel
{
    private readonly IUserRepository _userRepository;

    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = String.Empty;
    public string Error { get; set; } = string.Empty;

    public LoginModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (string.IsNullOrEmpty(Username))
            Error = "You have to enter a username";
        else if (string.IsNullOrEmpty(Password))
            Error = "You have to enter a password";
        else
        {
            var user = _userRepository.Login(Username, Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.name),
                    new Claim(ClaimTypes.Email, user.email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), 
                    authProperties);
                
                TempData["FlashMessage"] = "You were logged in";
                return Redirect("/");
            }
            else
            {
                Error = "Invalid username or password";
            }
        }
        return Page();
    }
}