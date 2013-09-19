using System;
namespace WebForms.Domain.DomainModel.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
    }
}
