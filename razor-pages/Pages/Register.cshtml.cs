using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class RegisterModel : PageModel
{
    private readonly IDBContext _dbcontext;
    public RegisterModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void OnGet()
    {
    }
}