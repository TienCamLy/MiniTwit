using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class LoginModel : PageModel
{
    private readonly IDBContext _dbcontext;
    public LoginModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    
    public void OnGet()
    {
        Console.WriteLine(_dbcontext.GetPublicTimeline(30));
    }
}