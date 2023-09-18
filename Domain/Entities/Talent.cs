using System;

namespace Domain.Entities
{
    public class Talent
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string ResumeUniqueName { get; set; }
        public string ResumeFileName { get; set; }
        public string Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public virtual User UserWhoUpdated { get; set; }
    }
}