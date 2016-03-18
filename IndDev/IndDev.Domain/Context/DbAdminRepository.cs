using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndDev.Auth.Logic;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Menu;

namespace IndDev.Domain.Context
{
    public class DbAdminRepository : IAdminRepository
    {
        private readonly DataContext _context= new DataContext();

        public IEnumerable<UsrRoles> Roleses => _context.Roles.ToList();

        public Customer Customer(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public ValidationInfo UpdateUser(User user)
        {
            var dbUser = User(user.Id);
            if (dbUser==null)
            {
                return new ValidationInfo {Code = -1, Message = "Ошибка. Такого пользователя нет в базе данных."};
            }
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.UsrRoles.Id);
            dbUser.Name = user.Name;
            dbUser.Phone = user.Phone;
            dbUser.Region = user.Region;
            dbUser.ConfirmEmail = user.ConfirmEmail;
            dbUser.Block = user.Block;
            dbUser.UsrRoles = role;
            _context.SaveChanges();
            return new ValidationInfo {Code = user.Id, Message = "Данные успешно обновлены"};
        }

        public IEnumerable<ProductMenu> ProductMenus()
        {
            return _context.ProductMenus.ToList();
        }

        public User User(int id)
        {
            var user = _context.Users.Find(id);
            if (user.Customer != null) return user;
            var cust = new Customer
            {
                Title = "No Name",
                Adress = user.Region
            };
            _context.Customers.Add(cust);
            user.Customer = cust;
            _context.SaveChanges();
            return user;
        }

        public IEnumerable<User> Users()
        {
            return _context.Users.ToList();
        }

        public void AddCategory(ProductMenu menu)
        {
            _context.ProductMenus.Add(menu);
            _context.SaveChanges();
        }

        public void DeleteCatImage(ProdMenuImage image)
        {
            if (image == null) return;
            if (File.Exists(image.FullPath))
            {
                File.Delete(image.FullPath);
            }
        }

        public ValidEvent RemoveCategory(int id)
        {
            var dbProMenu = _context.ProductMenus.Find(id);
            if (dbProMenu.MenuItems.Any())
            {
                return new ValidEvent
                {
                    Code = dbProMenu.MenuItems.Count,
                    Messge = $"Нельзя удалить не пустую категорию. Сначала удалите {dbProMenu.MenuItems.Count} подкатегорий"
                };
            }
            if (dbProMenu.Image!=null)
            {
                DeleteCatImage(dbProMenu.Image);
            }
            _context.ProductMenus.Remove(dbProMenu);
            var ve = new ValidEvent
            {
                Code = dbProMenu.Id,
                Messge = $"Категоря {dbProMenu.Title} успешно удалена."
            };
            _context.SaveChanges();
            return ve;
        }

        public ProductMenu GetProductMenu(int id)
        {
           return _context.ProductMenus.Find(id);
        }

        public void AddSubCategory(ProductMenuItem item)
        {
            _context.ProductMenuItems.Add(item);
            _context.SaveChanges();
        }

        public ValidEvent UpdateCategory(ProductMenu menu, ProdMenuImage image)
        {
            var dbMenu = _context.ProductMenus.Find(menu.Id);
            dbMenu.Descr = menu.Descr;
            dbMenu.Title = menu.Title;
            dbMenu.Priority = menu.Priority;
            if (image!=null)
            {
                if (dbMenu.Image!=null)DeleteCatImage(dbMenu.Image);
                dbMenu.Image = image;
            }
            _context.SaveChanges();
            return new ValidEvent {Code = dbMenu.Id, Messge = "Данные изменены."};
        }

        public ProductMenuItem GetSubCat(int id)
        {
            return _context.ProductMenuItems.Find(id);
        }

        public ValidEvent UpdateSubCategory(ProductMenuItem menu, ProdMenuImage image)
        {
            var dbMenu = _context.ProductMenuItems.Find(menu.Id);
            dbMenu.Descr = menu.Descr;
            dbMenu.Title = menu.Title;
            dbMenu.IsRus = menu.IsRus;
            if (image != null)
            {
                if (dbMenu.Image != null) DeleteCatImage(dbMenu.Image);
                dbMenu.Image = image;
            }
            _context.SaveChanges();
            return new ValidEvent { Code = dbMenu.Id, Messge = "Данные изменены." };
        }

        public ValidEvent RemoveSubCat(int id)
        {
            var dbSc = _context.ProductMenuItems.Find(id);
            if (dbSc.Products!=null && dbSc.Products.Any()) return new ValidEvent {Code = 0,Messge = "Нельзя удалить категорию с товарами. Сначала удалите все товары в категории."};

            DeleteCatImage(dbSc.Image);
            _context.ProductMenuItems.Remove(dbSc);
            _context.SaveChanges();

            return new ValidEvent {Code = dbSc.Id, Messge = "Категория удалена."};
        }

        public IEnumerable<Menu> GetrootMenuItems()
        {
            return _context.Menus.Where(menu => menu.ParentItem == null).ToList();
        }

        public IEnumerable<Menu> GetSubMenuItems(int id)
        {
            return _context.Menus.Where(menu => menu.ParentItem.Id == id).ToList();
        }

        public Menu GetMenu(int id)
        {
            if (id != 0) return _context.Menus.Find(id);
            var i = _context.Menus.Min(menu => menu.Id);
            return _context.Menus.FirstOrDefault(menu => menu.Id==i);
        }

        public void AddSubMenuItem(Menu menu)
        {
            var pMenu = _context.Menus.Find(menu.Id);
            pMenu.HasChild = true;
            var newMenu = new Menu
            {
                HasChild = false,
                ParentItem = pMenu,
                Title = menu.Title
            };
            _context.Menus.Add(newMenu);
            _context.SaveChanges();
        }

        public void RemoveSubMenu(int id)
        {
            var dbMenu = _context.Menus.Find(id);
            var parentId = dbMenu.ParentItem.Id;
            if (dbMenu.HasChild) return;
            _context.Menus.Remove(dbMenu);
            var dbParent = _context.Menus.Find(parentId);
            if (_context.Menus.Any(menu => menu.ParentItem.Id==dbParent.Id))
            {
                dbParent.HasChild = false;
            }
            _context.SaveChanges();
        }
    }
}