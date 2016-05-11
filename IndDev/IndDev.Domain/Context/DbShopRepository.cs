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

        public IEnumerable<ShopProductView> GetProducts(int catId, int selCat)
        {
            var pvm = new List<ShopProductView>();
            var sMenuItem = _context.ProductMenuItems.Find(catId);
            //if (selCat!=0)
            //{
            //    var products = _context.Products.Where(product => product.Categoy.Id == selCat).ToList();
            //    foreach (var product in products)
            //    {
            //        var pv = new ShopProductView
            //        {
            //            Product = product,
            //            Avatar =
            //                product.ProductPhotos.Any(photo => photo.PhotoType == PhotoType.Avatar)
            //                    ? product.ProductPhotos.FirstOrDefault(photo => photo.PhotoType == PhotoType.Avatar)
            //                    : new ProductPhoto {AltText = "нет фотки", Path = "/Content/images/noimage.png"}
            //        };
            //        var pr = product.Prices.Select(price => new PriceViewModel {Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom}).ToList();
            //        pv.Prices = pr;
            //        pv.IsSale = IsSale(pr);
            //        pv.Byeble = CheckPrice(pr);
            //        pv.SubCategory = catId;
            //        pvm.Add(pv);
            //    }
            //    return pvm;
            //}
            //var sCat = _context.ProductMenus.Find(catId).MenuItems.ToList();
            //foreach (var p in sCat.Select(item => _context.Products.Where(product => product.Categoy.Id == item.Id).ToList()))
            //{
            //    foreach (var product in p)
            //    {
            //        var pv = new ShopProductView
            //        {
            //            Product = product,
            //            Avatar =
            //                product.ProductPhotos.Any(photo => photo.PhotoType == PhotoType.Avatar)
            //                    ? product.ProductPhotos.FirstOrDefault(photo => photo.PhotoType == PhotoType.Avatar)
            //                    : new ProductPhoto { AltText = "нет фотки", Path = "/Content/images/noimage.png" }
            //        };
            //        var pr = product.Prices.Select(price => new PriceViewModel { Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom}).ToList();
            //        pv.Prices = pr;
            //        pv.Byeble = CheckPrice(pr);
            //        pv.IsSale = IsSale(pr);
            //        pv.SubCategory = catId;
            //        pvm.Add(pv);
            //    }
            //}
            return pvm;
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

        public ShopProductView GetProduct(int id) //menu item 
        {
            //var product = _context.Products.Find(id);
            //var pv = new ShopProductView();
            //{
            //    Product = product,
            //    Avatar =
            //        product.ProductPhotos.Any(photo => photo.PhotoType == PhotoType.Avatar)
            //            ? product.ProductPhotos.FirstOrDefault(photo => photo.PhotoType == PhotoType.Avatar)
            //            : new ProductPhoto { AltText = "нет фотки", Path = "/Content/images/noimage.png" }
            //};
            //var pr = product.Prices.Where(price => price.Value>0).Select(price => new PriceViewModel { Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom}).ToList();
            //pv.Prices = pr;
            //pv.Byeble = CheckPrice(pr);
            //pv.IsSale = IsSale(pr);
            //pv.SubCategory = mCat;
            var model = new ShopProductView
            {
                ProductMenuItem = _context.ProductMenuItems.Find(id)
            };
            return model;
        }

        public ShopProductView GetProduct(int id, int mCat)
        {
            throw new NotImplementedException();
        }
    }
}