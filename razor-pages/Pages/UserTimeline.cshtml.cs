using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IDBContext _dbcontext;
    public UserTimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void OnGet()
    {
    }
}