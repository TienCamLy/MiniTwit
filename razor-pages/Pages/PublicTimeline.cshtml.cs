using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_pages.Structs;

namespace razor_pages.Pages;

public class PublicTimelineModel : PageModel
{
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
    private readonly IDBContext _dbcontext;
    public PublicTimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void OnGet()
    {
        Messages = _dbcontext.GetPublicTimeline(30);
    }
}