using System.Collections.Generic;

namespace Domain.Entities
{
    public class Profile
    {
        public Profile()
        {
            UserList = new HashSet<User>();
            ProfilePermissions = new HashSet<ProfilePermission>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public virtual ICollection<User> UserList { get; set; }
        public virtual ICollection<ProfilePermission> ProfilePermissions { get; set; }
    }
}
