using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_pages.Structs;
using System.Globalization;

namespace razor_pages.Pages;

public class TimelineModel : PageModel
{
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
    private readonly IDBContext _dbcontext;
    public TimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void OnGet()
    {
        Messages = _dbcontext.GetPublicTimeline(30);
    }
}