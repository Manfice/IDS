using IndDev.Domain.Entity.Cart;

namespace IndDev.Domain.ViewModels
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } 
    }
}