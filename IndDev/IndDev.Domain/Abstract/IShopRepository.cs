using System.Collections.Generic;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface IShopRepository
    {
        IEnumerable<Menu> GetTopMenus();
        IEnumerable<Menu> GetSubMenu(int id);//Curent menu ID
        IEnumerable<ProductMenu> GetProductMenus();
        ProductMenu GetProductMenu(int id);
        IEnumerable<ShopProductView> GetProducts(int catId, int selCat);
        ShopProductView GetProduct(int id, int mCat);
        ShopProductView GetProduct(int id);
    }
}