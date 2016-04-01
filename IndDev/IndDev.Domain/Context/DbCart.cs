using System;
using IndDev.Domain.Abstract;

namespace IndDev.Domain.Context
{
    public class DbCart : ICartRepository
    {
        public void AddToCart(int prodId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}