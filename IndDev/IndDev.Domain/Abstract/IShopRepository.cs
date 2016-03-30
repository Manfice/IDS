using System.Collections.Generic;
using IndDev.Domain.Entity.Menu;

namespace IndDev.Domain.Abstract
{
    public interface IShopRepository
    {
        IEnumerable<Menu> GetTopMenus();
        IEnumerable<Menu> GetSubMenu(int id);//Curent menu ID
        IEnumerable<ProductMenu> GetProductMenus();
        ProductMenu GetProductMenu(int id);
    }
}