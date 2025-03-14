using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace BlazorAuthFromScratch.Services
{
    public class BlazorAuthFromScratchClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal?.Identity?.AuthenticationType == "Microsoft")
            {
                if (principal.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
                {
                    // theoretically we could use the graph api here to
                    // get more information about the user, but we don't have
                    // the user's access token, so we can't use delegated permissions
                    // here.  We could give the app permission to read profiles in the tenant
                    // and look up by name identifier, but that might be considered more
                    // privileged than necessary.
                }
            }

            // add new claims this way:

            //ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            //var claimType = "myNewClaim";
            //if (!principal.HasClaim(claim => claim.Type == claimType))
            //{
            //    claimsIdentity.AddClaim(new Claim(claimType, "myClaimValue"));
            //}

            //principal.AddIdentity(claimsIdentity);
            return Task.FromResult(principal!);
        }
    }
}
