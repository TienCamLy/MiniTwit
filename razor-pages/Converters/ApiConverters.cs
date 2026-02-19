using Org.OpenAPITools.Models;
using razor_pages.Structs;

namespace razor_pages.Converters;

/// <summary>
/// Converts domain structs to API (OpenAPI) DTOs for the Minitwit API.
/// </summary>
public static class ApiConverters
{
    /// <summary>API pub_date format matching the spec (e.g. "2019-12-01 12:00:00").</summary>
    private const string PubDateFormat = "yyyy-MM-dd HH:mm:ss";

    public static Org.OpenAPITools.Models.Message ToApiMessage(Structs.Message message)
    {
        return new Org.OpenAPITools.Models.Message
        {
            Content = message.text,
            PubDate = message.pub_date.ToString(PubDateFormat),
            User = message.author.name
        };
    }
}
