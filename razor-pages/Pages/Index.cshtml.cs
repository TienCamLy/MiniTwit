using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class IndexModel : PageModel
{
    private readonly IDBContext _dbcontext;
    public IndexModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void OnGet()
    {
    }
}