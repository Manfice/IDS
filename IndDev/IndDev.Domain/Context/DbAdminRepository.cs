﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using IndDev.Auth.Logic;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.Entity.Stock;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class DbAdminRepository : IAdminRepository
    {
        private readonly DataContext _context= new DataContext();

        public IEnumerable<UsrRoles> Roleses => _context.Roles.ToList();

        public IEnumerable<CustomerStatus> GetCustomerStatuses => _context.CustomerStatuses.ToList();

        public IEnumerable<Customer> GetCustomers => _context.Customers.ToList();

        public IEnumerable<Stock> GetStocks => _context.Stocks.ToList();

        public Customer Customer(int id)
        {
            return _context.Customers.Find(id);
        }

        public ValidationInfo UpdateUser(User user)
        {
            using (var ctx = new DataContext())
            {
                var role = ctx.Roles.Find(user.UsrRoles.Id);
                var usr = ctx.Users.Find(user.Id);
                usr.Name = user.Name;
                usr.Phone = user.Phone;
                usr.Region = user.Region;
                usr.ConfirmEmail = user.ConfirmEmail;
                usr.Block = user.Block;
                usr.UsrRoles = role;
                ctx.SaveChanges();
            }
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
            var stock = _context.Stocks.Find(model.StockId);
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
                Warranty = model.Product.Warranty,
                Stock = stock
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
            photo.Product = product;
            _context.ProductPhotos.Add(photo);
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
                PriceType = price.PriceType,
                Currencies = _context.Currencies.Select(currency => new SelectListItem {Text = currency.StringCode, Value = currency.Id.ToString()}),
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
            return new ValidEvent {Code = dbPrice.Product.Id};
        }

        public Product UpdateProduct(ProductDetailsViewModel model)
        {
            var dbProduct = _context.Products.Find(model.Product.Id);
            var brand = _context.Brands.Find(model.SelBr);
            var vendor = _context.Vendors.Find(model.SelVr);
            var mu = _context.MesureUnits.Find(model.SelMu);
            var stock = _context.Stocks.Find(model.SelStock);
            dbProduct.Warranty = model.Product.Warranty;
            dbProduct.Articul = model.Product.Articul;
            dbProduct.Title = model.Product.Title;
            dbProduct.Description = model.Product.Description;
            dbProduct.IsService = model.Product.IsService;
            dbProduct.Reclama = model.Product.Reclama;
            dbProduct.Brand = brand;
            dbProduct.Vendor = vendor;
            dbProduct.MesureUnit = mu;
            dbProduct.Stock = stock;
            dbProduct.Warning = model.Product.Warning;
            foreach (var price in model.Prices)
            {
                var dbPrice = _context.Prices.Find(price.Id);
                dbPrice.Currency = _context.Currencies.Find(price.SelCurr);
                dbPrice.Title = price.Title;
                dbPrice.Value = price.Value;
            }
            _context.SaveChanges();
            return dbProduct;
        }

        public Customer SaveCustomer(EditCustomer model)
        {
            var customer = _context.Customers.Find(model.Id);
            if (string.IsNullOrEmpty(customer.Adress))
            {
                customer.Adress = model.Details.RealAdress;
            }
            var dbDetails = _context.Detailses.Find(model.Details.Id);
            dbDetails.CompanyName = model.Details.CompanyName;
            dbDetails.Inn = model.Details.Inn;
            dbDetails.Kpp = model.Details.Kpp;
            dbDetails.UrAdress = model.Details.UrAdress;
            dbDetails.RealAdress = model.Details.RealAdress;
            dbDetails.Ogrn = model.Details.Ogrn;
            var cStat = _context.CustomerStatuses.Find(model.Status);
            customer.CustomerStatus = cStat;
            _context.SaveChanges();
            return customer;
        }

        public List<PriceSetter> GetPrices(int prodId)
        {
            var prs = _context.Products.Find(prodId).Prices.ToList();
            var psl = new List<PriceSetter>();
            foreach (var price in prs)
            {
                var ps = new PriceSetter
                {
                    Value = price.Value,
                    Title = price.Title,
                    Id = price.Id,
                    PriceType = price.PriceType,
                    Currencies = _context.Currencies.Select(currency => new SelectListItem {Text = currency.StringCode, Value = currency.Id.ToString()}),
                    PriceFrom = 1,
                    Public = price.Publish,
                    SelCurr = price.Currency.Id,
                    Valuta = price.Currency.Code
                };
                psl.Add(ps);
            }
            return psl;
        }

        public ValidEvent RemoveImage(int id)
        {
            var dbImage = _context.ProductPhotos.Find(id);
            var result = new ValidEvent {Code = dbImage.Product.Id, Messge = "Ok"};
            _context.ProductPhotos.Remove(dbImage);
            _context.SaveChanges();
            return result;
        }

        public void RemovePhoto(int id)
        {
            var img = _context.ProductPhotos.Find(id);
            _context.ProductPhotos.Remove(img);
            _context.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetValutes()
        {
            return
                _context.Currencies.Select(
                    currency => new SelectListItem {Text = currency.StringCode, Value = currency.Id.ToString()});
        }
    }
}