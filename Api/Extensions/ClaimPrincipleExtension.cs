using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimPrincipleExtension
{
    public static string GetUsername(this ClaimsPrincipal user) {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get token from the username");
        return username;
    }
}
