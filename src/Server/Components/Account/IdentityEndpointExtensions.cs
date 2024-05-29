using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Routing;

internal static class IdentityEndpointExtensions
{
    public static IEndpointConventionBuilder MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");

        accountGroup.MapGet("/Logout", async (HttpContext context) =>
        {
            await context.SignOutAsync("TdtsCookie");
            return TypedResults.LocalRedirect($"/");
        });

        accountGroup.MapPost("/PerformGoogleLogin", async (
            HttpContext context,
            [FromForm] string returnUrl
        ) =>
        {
            // a more generic implementation would pass the provider as well but we know it's Google here
            string provider = "Google";

            var properties = new AuthenticationProperties { RedirectUri = returnUrl };

            // Sign out any existing user
            await context.SignOutAsync("TdtsCookie");

            return TypedResults.Challenge(properties, [provider]);
        });

        accountGroup.MapPost("/PerformMicrosoftLogin", async (
             HttpContext context,
             [FromForm] string returnUrl
             ) =>
        {
            // a more generic implementation would pass the provider as well but we know it's Google here
            string provider = "Microsoft";

            var properties = new AuthenticationProperties { RedirectUri = returnUrl };

            // Sign out any existing user
            await context.SignOutAsync("TdtsCookie");

            return TypedResults.Challenge(properties, [provider]);
        });

        return accountGroup;
    }
}

