using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Model
{
    public class ProfilePermission : IdentityUserRole<int>
    {
        public CustomRole Profile { get; set; }
        public CustomClaim Permission { get; set; }
        public int IdPermission { get; set; }

    }
}
