using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Abstract
{
    public interface ICartRepository
    {
        Product GetProduct(int id);
    }
}