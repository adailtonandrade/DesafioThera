using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Identity.Model
{
    public class CustomRole : IdentityRole<int, ProfilePermission>
    {
        public List<ApplicationUser> UserList { get; set; }
        public List<ProfilePermission> ProfilePermissions { get; set; }
    }
}
