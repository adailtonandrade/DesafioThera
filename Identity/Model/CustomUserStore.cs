using Identity.Context;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Model
{
    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, ProfilePermission, CustomClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}