using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class DbShopRepository:IShopRepository
    {
         private readonly DataContext _context = new DataContext();

        public ProductMenu GetProductMenu(int id)
        {
            return _context.ProductMenus.Find(id);
        }

        private bool IsSale(List<PriceViewModel> model)
        {
            return model.Any(item => item.Title == "Sale" && item.ConvValue > 0);
        }

        private bool CheckPrice(List<PriceViewModel> list)
        {
            return list != null && list.Any(item => item.ConvValue>0);
        }

        public IEnumerable<ProductMenu> GetProductMenus()
        {
            return _context.ProductMenus.ToList().OrderBy(menu => menu.Priority);
        }

        public IEnumerable<Menu> GetSubMenu(int id)
        {
            return _context.Menus.Where(menu => menu.ParentItem.Id==id).ToList();
        }

        public IEnumerable<Menu> GetTopMenus()
        {
            return _context.Menus.Where(menu => menu.ParentItem==null).ToList();
        }

        public ShopProductView GetProduct(int id) //menu item ID
        {
            var menuItem = _context.ProductMenuItems.Find(id);
            var products = menuItem.Products.Select(product => new ProductView
            {
                Product = product, Avatar = product.ProductPhotos.FirstOrDefault(p => p.PhotoType == PhotoType.Avatar), Prices = product.Prices.Where(price => price.Value > 0).Select(price => new PriceViewModel {Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom, PriceType = price.PriceType}).ToList()
            }).ToList();

            return new ShopProductView {ProductMenuItem = menuItem, Products = products};
        }

        public ProductView GetProductDetails(int id)
        {
            var prod = _context.Products.Find(id);
            var pvm = new ProductView
            {
                Product = prod,
                Avatar = prod.ProductPhotos.FirstOrDefault(p => p.PhotoType == PhotoType.Avatar),
                Prices = prod.Prices.Where(price => price.Value > 0).Select(price => new PriceViewModel { Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom, PriceType = price.PriceType}).ToList()
            };
            return pvm;
        }
    }
}