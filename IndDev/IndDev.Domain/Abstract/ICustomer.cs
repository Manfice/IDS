using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Cart;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Orders;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface ICustomer
    {
        Customer GetCustomerByUserId(int id);
        User GetUserById(int id);
        ValidEvent UpdateCustomer(User model);
        Order MakeOrder(int userId, Cart cart, PreOrder preOrder);
        List<DeliveryType> GetDeliveryTypes();
        IEnumerable<Order> GetOrders(int custId);
        Order GetOrderById(int id);
    }
}