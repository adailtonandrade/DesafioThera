using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Model
{
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, ProfilePermission, CustomClaim>
    {
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Active { get; set; }
        public int IdProfile { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomRole Profile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            Claim nickname = new Claim("NickName", this.NickName);
            Claim name = new Claim("Name", this.Name);
            // Add custom user claims here
            userIdentity.AddClaim(nickname);
            userIdentity.AddClaim(name);
            return userIdentity;
        }
    }
}
