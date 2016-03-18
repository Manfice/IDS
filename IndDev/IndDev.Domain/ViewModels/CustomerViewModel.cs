using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.ViewModels
{
    public class CustomerViewModel
    {
        public User User { get; set; }
        public Customer Customer { get; set; } 
    }
}