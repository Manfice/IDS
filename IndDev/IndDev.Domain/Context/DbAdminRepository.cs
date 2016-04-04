using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndDev.Auth.Logic;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

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

        public ValidEvent AddCategory(ProductMenu menu)
        {
            var cat = menu;
            _context.ProductMenus.Add(cat);
            _context.SaveChanges();
            return new ValidEvent {Code = cat.Id};
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
            if (menu.Id==0)
            {
                var rootMenu = new Menu
                {
                    HasChild = false,
                    ParentItem = null,
                    Title = menu.Title
                };
                _context.Menus.Add(rootMenu);
                _context.SaveChanges();
                return;
            }
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
            _context.SaveChanges();
            var prCount = _context.Menus.Where(menu => menu.ParentItem.Id==parentId).ToList();
            if (prCount.Count==0)
            {
                var dbParent = _context.Menus.Find(parentId);
                dbParent.HasChild = false;
            }
            _context.SaveChanges();
        }

        public IEnumerable<ProductViewModel> GetProductsByMenu(int menuId)
        {
            var pvm = new List<ProductViewModel>();
            var products = _context.Products.Where(product => product.MenuItem.Id == menuId).ToList();
            foreach (var product in products)
            {
                var p = new ProductViewModel
                {
                    Id = product.Id,
                    Title = product.Title,
                   // PriceIn = product.Prices.FirstOrDefault(price => price.PriceType==PriceType.InputPrice).Value,
                    Currency = product.Prices.FirstOrDefault(price => price.PriceType==PriceType.LowOpt).Currency.StringCode
                  //  PriceOut = product.Prices.FirstOrDefault(price => price.PriceType==PriceType.OutputPrice).Value
                };
                pvm.Add(p);
            }
            return pvm;
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _context.Brands.ToList();
        }

        public IEnumerable<MesureUnit> GetAllMesureUnits()
        {
            return _context.MesureUnits.ToList();
        }

        public IEnumerable<Vendor> GetAllVendors()
        {
            return _context.Vendors.ToList();
        }

        public ValidEvent SaveProduct(AddProductViewModel model)
        {
            var brand = _context.Brands.Find(model.SelBrand);
            var mu = _context.MesureUnits.Find(model.SelMU);
            var cat = _context.ProductMenuItems.Find(model.SubCatId);
            var product = new Product
            {
                Articul = model.Product.Articul,
                Brand = brand,
                MesureUnit = mu,
                Categoy = cat,
                Description = model.Product.Description,
                IsService = model.Product.IsService,
                Title = model.Product.Title,
                Vendor = brand.Vendor,
                Warranty = model.Product.Warranty
            };
            _context.Products.Add(product);
            var pTypes = Enum.GetNames(typeof (PriceType));
            foreach (var s in pTypes)
            {
                var price = new Price
                {
                    PriceType = (PriceType)Enum.Parse(typeof(PriceType),s),
                    Currency = _context.Currencies.FirstOrDefault(currency => currency.Code=="RUB"),
                    Product = product,
                    Publish = false,
                    Title = s,
                    Value = 0,
                    QuanttityFrom = 1
                };
                _context.Prices.Add(price);
            }
            _context.SaveChanges();
            return new ValidEvent {Code = product.Id};
        }

        public IEnumerable<Product> GetCatProduct(int catId)
        {
            return _context.Products.Where(product => product.Categoy.Id == catId).ToList();
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Find(id);
        }

        public ValidEvent RemoveProduct(int id)
        {
            //Todo:Проверить удаление цен.
            var prod = _context.Products.Find(id);
            var cat = prod.Categoy.Id;
            var prs = prod.Prices.ToList();
            var img = prod.ProductPhotos;
            foreach (var price in prs)
            {
                var dbPrice = _context.Prices.Find(price.Id);
                _context.Prices.Remove(dbPrice);
            }
            _context.ProductPhotos.RemoveRange(img);
            _context.Products.Remove(prod);
            _context.SaveChanges();
            return new ValidEvent {Code = cat};
        }

        public ValidEvent AddPhotoToProduct(int prodId, ProductPhoto photo)
        {
            var product = _context.Products.Find(prodId);
            var avatar = product.ProductPhotos.Count(productPhoto => productPhoto.PhotoType == PhotoType.Avatar);
            var image = new ProductPhoto
            {
                AltText = photo.AltText,
                FullPath = photo.FullPath,
                PhotoType = avatar>0? PhotoType.Photo : PhotoType.Avatar,
                Path = photo.Path,
                Product = product
            };
            _context.ProductPhotos.Add(image);
            _context.SaveChanges();
            return new ValidEvent {Code = product.Id};
        }

        public IEnumerable<PriceViewModel> GetProdPrices(int prodId)
        {
            var product = _context.Products.Find(prodId);
            var curses = new Valutes(DateTime.Today).GetCurses();
            var pvml = new List<PriceViewModel>();
            foreach (var price in product.Prices)
            {
                var p = new PriceViewModel
                {
                    Product = product,
                    Currency = price.Currency,
                    Title = price.Title,
                    OriginalPrice = price.Value
                };
                pvml.Add(p);
            }
            return pvml;
            
        }

        private decimal ValToRub(decimal value, decimal curs)
        {
            return value>0 ? value*curs:0;
        }
        public PriceSetter GetPriceSetter(int id)
        {
            var price = _context.Prices.Find(id);
            var pvm = new PriceViewModel
            {
                Currency = price.Currency,
                Product = price.Product,
                Title = price.Title,
                OriginalPrice = price.Value
            };
            return new PriceSetter
            {
                PriceViewModel = pvm,
                PriceType = price.PriceType,
                Currencies = _context.Currencies.ToList(),
                Public = price.Publish,
                Title = price.Title,
                Value = price.Value,
                Id = price.Id,
                PriceFrom = price.QuanttityFrom
            };
        }

        public ValidEvent SetPrice(PriceSetter model)
        {
            var dbPrice = _context.Prices.Find(model.Id);
            dbPrice.Title = model.Title;
            dbPrice.Value = model.Value;
            dbPrice.QuanttityFrom = model.PriceFrom;
            if (model.SelCurr != 0)
            {
                var curr = _context.Currencies.Find(model.SelCurr);
                dbPrice.Currency = curr;
            }
            _context.SaveChanges();
            return new ValidEvent {Code = model.PriceViewModel.Product.Id};
        }

        public Product UpdateProduct(Product model)
        {
            var dbProduct = _context.Products.Find(model.Id);
            dbProduct.Title = model.Title;
            dbProduct.Articul = model.Articul;
            dbProduct.Description = model.Description;
            dbProduct.IsService = model.IsService;
            _context.SaveChanges();
            return dbProduct;
        }
    }
}