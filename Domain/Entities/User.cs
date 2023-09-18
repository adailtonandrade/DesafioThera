using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public User()
        {
            UserLoginList = new HashSet<UserLogin>();
        }
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public string Username { get; set; }
        public string NickName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUTC { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<UserLogin> UserLoginList { get; set; }
        public virtual ICollection<Talent> Applicants { get; set; }
    }
}