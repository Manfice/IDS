using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.ViewModels
{
    public class CustomerViewModel
    {
        public User User { get; set; }
        public Customer Customer { get; set; } 
    }

    public class CustMenuItems
    {
        public string Title { get; set; }
        public string MenuLink { get; set; }
        public bool IsActive { get; set; }
    }
}