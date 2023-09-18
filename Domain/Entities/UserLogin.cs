namespace Domain.Entities
{
    public class UserLogin
    {
        public int IdUser { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public virtual User User { get; set; }
    }
}
