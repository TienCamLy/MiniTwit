using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor_pages.Pages;

public class TimelineModel : PageModel
{
    private readonly IDBContext _dbcontext;
    public TimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void OnGet()
    {
    }
}