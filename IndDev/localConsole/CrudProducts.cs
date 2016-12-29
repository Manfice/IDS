using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Context;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;

namespace localConsole
{
    public class CrudProducts
    {
        private readonly DataContext _dbContext;

        public CrudProducts()
        {
            _dbContext = new DataContext();
        }

        public string DeleteProduct(int id)
        {
            var p = _dbContext.Products.Find(id);

            if (p==null)
            {
                return "Product was not found";
            }
            _dbContext.Prices.RemoveRange(p.Prices);
            _dbContext.Products.Remove(p);
            _dbContext.SaveChanges();
            return $"Product with id {id} was successed deleted from database";
        }

        public List<Product> GetProductsWithNullArtOrTitle()
        {
            var result =
                _dbContext.Products.Where(
                    product => string.IsNullOrEmpty(product.Articul) || string.IsNullOrEmpty(product.Title)).ToList();
            return result;
        }

        public List<ProductMenu> GetProducCategorys()
        {
            return _dbContext.ProductMenus.ToList();
        }


    }
}