using System.Collections.Generic;
using System.Web.Mvc;
using IndDev.Domain.Entity.Cart;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Orders;

namespace IndDev.Domain.ViewModels
{
    public class OrderViewModels
    {
         
    }

    public class PreOrder
    {
        public Cart Cart { get; set; }
        public Customer Customer { get; set; }
        public Delivery Delivery { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<SelectListItem> DeliveryTypes { get; set; }
    }
}