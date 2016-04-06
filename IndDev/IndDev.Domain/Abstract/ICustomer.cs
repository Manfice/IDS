using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Abstract
{
    public interface ICustomer
    {
        Customer GetCustomerByUserId(int id);
    }
}