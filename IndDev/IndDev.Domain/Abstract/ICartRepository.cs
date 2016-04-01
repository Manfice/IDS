namespace IndDev.Domain.Abstract
{
    public interface ICartRepository
    {
        void AddToCart(int prodId, int quantity);
    }
}