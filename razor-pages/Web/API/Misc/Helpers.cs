using System.Security.Cryptography;
using System.Text;

namespace Web.API.Misc;

public static class Helpers
{
    // Return the gravatar image for the given email address.
    public static string GenerateGravatarUrl(string email)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(email.Trim().ToLowerInvariant());
        var hashBytes = md5.ComputeHash(inputBytes);
        
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
            sb.Append(b.ToString("x2"));

        return $"https://www.gravatar.com/avatar/{sb}?d=identicon&s=48";
    }
}