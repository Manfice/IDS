using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Menu;

namespace IndDev.Domain.Context
{
    public class DbShopRepository:IShopRepository
    {
         private readonly DataContext _context = new DataContext();

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
    }
}