using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Routing;

internal static class IdentityEndpointExtensions
{
    public static IEndpointConventionBuilder MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");

        accountGroup.MapGet("/Logout", async (HttpContext context) => {
            await context.SignOutAsync("TdtsCookie");
            return TypedResults.LocalRedirect($"/");
        });

        return accountGroup;
    }
}

