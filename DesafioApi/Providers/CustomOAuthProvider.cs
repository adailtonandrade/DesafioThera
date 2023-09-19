using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DesafioApi.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Validate the user's credentials (e.g., username and password)
            // You can replace this with your own logic to validate users
            if (context.UserName == "yourUsername" && context.Password == "yourPassword")
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

                // Add additional claims here if needed

                var ticket = new AuthenticationTicket(identity, null);
                context.Validated(ticket);
            }
            else
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                context.Rejected();
            }
        }
    }