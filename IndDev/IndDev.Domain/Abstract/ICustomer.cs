using System.Threading.Tasks;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Abstract
{
    public interface ICustomer
    {
        Customer GetCustomerByUserId(int id);
        User GetUserById(int id);
        ValidEvent UpdateCustomer(User model);
    }
}