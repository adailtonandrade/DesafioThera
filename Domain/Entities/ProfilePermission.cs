namespace Domain.Entities
{
    public class ProfilePermission
    {

        public int ProfileId { get; set; }
        public int PermissionId { get; set; }
        public int? IdUser { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
