using System;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Entity.Auth

{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }  
        public bool ConfirmEmail { get; set; }
        public bool Block { get; set; }
        public string PasswordHash { get; set; }
        public Guid TempSecret { get; set; }
        public virtual UsrRoles UsrRoles { get; set; }
        public virtual Customer Customer { get; set; }
        
    }
}