﻿using System.Collections.Generic;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface IShopRepository
    {
        IEnumerable<ProductMenu> GetProductMenus { get; }
        IEnumerable<Product> GetProducts { get; }
        IEnumerable<Menu> GetTopMenus();
        IEnumerable<Menu> GetSubMenu(int id);//Curent menu ID
        ProductMenu GetProductMenu(int id);
        void SaveSearch(SearchRequests model);
        ProductView GetProductDetails(int id);
        ShopProductView GetProduct(int id);
    }
}