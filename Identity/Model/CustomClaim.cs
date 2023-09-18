using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
namespace Identity.Model
{
    public class CustomClaim : IdentityUserClaim<int>
    {
        public List<ProfilePermission> ProfilePermissions { get; set; }
    }
}
