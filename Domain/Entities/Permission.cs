using System.Collections.Generic;

namespace Domain.Entities
{
    public class Permission
    {
        public Permission()
        {
            ProfilePermissions = new HashSet<ProfilePermission>();
        }

        public int Id { get; set; }

        public int? IdUser { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; } 

        public virtual ICollection<ProfilePermission> ProfilePermissions { get; set; }

        public bool Validate()
        {
            return true;
        }
    }

}
