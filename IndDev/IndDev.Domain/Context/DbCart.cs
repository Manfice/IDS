using System;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Context
{
    public class DbCart : ICartRepository
    {
        private readonly DataContext _context = new DataContext();

        public Product GetProduct(int id)
        {
            return _context.Products.Find(id);
        }
    }
}