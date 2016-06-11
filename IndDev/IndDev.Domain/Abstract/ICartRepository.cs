using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Abstract
{
    public interface ICartRepository
    {
        Product GetProduct(int id);
        Customer GetCustomer(int id);//Set user Id not customer id
    }
}