using System;
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
using IndDev.Domain.Entity.Orders;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.Entity.Stock;
using IndDev.Domain.Logic;
using IndDev.Domain.ViewModels;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace IndDev.Domain.Context
{
    public class DbAdminRepository : IAdminRepository
    {
        private readonly DataContext _context= new DataContext();

        public IEnumerable<UsrRoles> Roleses => _context.Roles.ToList();

        public IEnumerable<CustomerStatus> GetCustomerStatuses => _context.CustomerStatuses.ToList();

        public IEnumerable<Customer> GetCustomers => _context.Customers.ToList();

        public IEnumerable<Stock> GetStocks => _context.Stocks.ToList();
        public void UpToDateProducts(IEnumerable<ProductExcell> products, int cat)
        {
            var productExcells = products as IList<ProductExcell> ?? products.ToList();
            if (!productExcells.Any()) return;
            foreach (var item in productExcells)
            {
                if (!string.IsNullOrWhiteSpace(item.Art))
                {
                    var product = _context.Products.FirstOrDefault(product1 => product1.Articul == item.Art);
                    if (product != null)
                    {
                        var currensy = _context.Currencies.Find(item.Curr);
                        if (product.Prices.Any(price => price.PriceType == PriceType.Retail))
                        {
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Retail).Currency = currensy;
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Retail).Value = item.Retail;
                        }
                        if (product.Prices.Any(price => price.PriceType == PriceType.LowOpt))
                        {
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.LowOpt).Currency = currensy;
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.LowOpt).Value = item.Opt;
                        }
                        if (product.Prices.Any(price => price.PriceType == PriceType.Opt))
                        {
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Opt).Currency = currensy;
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Opt).Value = item.Partner;
                        }
                        if (product.Prices.Any(price => price.PriceType == PriceType.Sale))
                        {
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Sale).Currency = currensy;
                            product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Sale).Value = item.Sale;
                        }
                        product.MesureUnit = _context.MesureUnits.Find(item.MeasureUnit);
                        product.Stock = _context.Stocks.Find(item.Stock);
                        product.Warranty = item.Warranty.ToString();
                        product.UpdateTime = DateTime.Now;
                        product.Warranty = item.Warranty.ToString();
                        product.Brand = _context.Brands.FirstOrDefault(p => p.FullName == item.Brand);
                        product.Title = item.Title;
                    }
                    else
                    {
                        var br = _context.Brands.FirstOrDefault(b => b.FullName.Contains(item.Brand));
                        var ct = _context.ProductMenuItems.Find(cat);
                        var p = new Product
                        {
                            Articul = item.Art,
                            Title = item.Title,
                            Brand = br,
                            Vendor = br?.Vendor,
                            Categoy = ct,
                            IsService = false,
                            Reclama = false,
                            UpdateTime = DateTime.Now,
                            Show = false,
                            MesureUnit = _context.MesureUnits.Find(item.MeasureUnit),
                            Stock = _context.Stocks.Find(item.Stock),
                            Warranty = item.Warranty.ToString()
                        };
                        _context.Products.Add(p);
                        var pTypes = Enum.GetNames(typeof (PriceType));
                        foreach (var s in pTypes)
                        {
                            var price = new Price
                            {
                                PriceType = (PriceType) Enum.Parse(typeof (PriceType), s),
                                Currency = _context.Currencies.Find(item.Curr),
                                Product = p,
                                Publish = false,
                                QuanttityFrom = 1
                            };
                            switch (price.PriceType)
                            {
                                case PriceType.Retail:
                                    price.Value = item.Retail;
                                    price.Title = "Розница";
                                    break;
                                case PriceType.LowOpt:
                                    price.Value = item.Opt;
                                    price.Title = "Опт";
                                    break;
                                case PriceType.Opt:
                                    price.Title = "Партнер";
                                    price.Value = item.Partner;
                                    break;
                                case PriceType.Sale:
                                    price.Title = "Распродажа";
                                    price.Value = item.Sale;
                                    break;
                            }
                            _context.Prices.Add(price);
                        }
                    }
                }
                else
                {
                    var br = _context.Brands.FirstOrDefault(b => b.FullName.Contains(item.Brand));
                    var ct = _context.ProductMenuItems.Find(cat);
                    var p = new Product
                    {
                        Title = item.Title,
                        Brand = br,
                        Vendor = br?.Vendor,
                        Categoy = ct,
                        IsService = false,
                        Reclama = false,
                        UpdateTime = DateTime.Now,
                        Show = false,
                        MesureUnit = _context.MesureUnits.Find(item.MeasureUnit),
                        Stock = _context.Stocks.Find(item.Stock),
                        Warranty = item.Warranty.ToString()
                    };
                    _context.Products.Add(p);
                    _context.SaveChanges();
                    p = _context.Products.Find(p.Id);
                    p.Articul = new ExternalLogic().GetArt(p.Id);
                    var pTypes = Enum.GetNames(typeof (PriceType));
                    foreach (var s in pTypes)
                    {
                        var price = new Price
                        {
                            PriceType = (PriceType) Enum.Parse(typeof (PriceType), s),
                            Currency = _context.Currencies.Find(item.Curr),
                            Product = p,
                            Publish = false,
                            QuanttityFrom = 1
                        };
                        switch (price.PriceType)
                        {
                            case PriceType.Retail:
                                price.Value = item.Retail;
                                price.Title = "Розница";
                                break;
                            case PriceType.LowOpt:
                                price.Value = item.Opt;
                                price.Title = "Опт";
                                break;
                            case PriceType.Opt:
                                price.Title = "Партнер";
                                price.Value = item.Partner;
                                break;
                            case PriceType.Sale:
                                price.Title = "Распродажа";
                                price.Value = item.Sale;
                                break;
                        }
                        _context.Prices.Add(price);
                    }
                }
                _context.SaveChanges();
            }
        }

        public void UpdateByPrice (IList<ProductExcell> priceItems)
        {
            if (priceItems.Any()) return;
            foreach (var item in priceItems)
            {
                if (string.IsNullOrWhiteSpace(item.Art)) continue;
                var product = _context.Products.FirstOrDefault(product1 => product1.Articul == item.Art);
                if (product == null) continue;
                var currensy = _context.Currencies.Find(item.Curr);
                if (product.Prices.Any(price => price.PriceType == PriceType.Retail))
                {
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Retail).Currency = currensy;
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Retail).Value = item.Retail;
                }
                if (product.Prices.Any(price => price.PriceType == PriceType.LowOpt))
                {
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.LowOpt).Currency = currensy;
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.LowOpt).Value = item.Opt;
                }
                if (product.Prices.Any(price => price.PriceType == PriceType.Opt))
                {
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Opt).Currency = currensy;
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Opt).Value = item.Partner;
                }
                if (product.Prices.Any(price => price.PriceType == PriceType.Sale))
                {
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Sale).Currency = currensy;
                    product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Sale).Value = item.Sale;
                }
                product.MesureUnit = _context.MesureUnits.Find(item.MeasureUnit);
                product.Stock = _context.Stocks.Find(item.Stock);
                product.Warranty = item.Warranty.ToString();
                product.UpdateTime = DateTime.Now;
                product.Warranty = item.Warranty.ToString();
                product.Brand = _context.Brands.FirstOrDefault(p => p.FullName == item.Brand);
                product.Title = item.Title;
            }
            _context.SaveChanges();
        }
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
                usr.Customer.CustomerStatus = ctx.CustomerStatuses.Find(user.Customer.CustomerStatus.Id);
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
            dbMenu.ShotDescription = menu.ShotDescription;
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
            dbMenu.Priority = menu.Priority;
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
                Stock = stock,
                Show = false,
                UpdateTime = DateTime.Now
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
                    Value = 0,
                    QuanttityFrom = 1
                };
                switch (price.PriceType)
                {
                        case PriceType.Sale:
                        price.Title = "Распродажа";break;
                    case PriceType.Retail:
                        price.Title = "Розница"; break;
                    case PriceType.LowOpt:
                        price.Title = "Опт"; break;
                    case PriceType.Opt:
                        price.Title = "Партнер"; break;
                }
                _context.Prices.Add(price);
            }
            _context.SaveChanges();
            return new ValidEvent {Code = product.Id};
        }

        public IEnumerable<Product> GetCatProduct(int catId)
        {
            return _context.Products.Where(product => product.Categoy.Id == catId).OrderByDescending(product => product.UpdateTime).ToList();
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
            dbProduct.UpdateTime = DateTime.Now;
            dbProduct.Show = model.Product.Show;
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
                    PriceType = price.PriceType,
                    Id = price.Id,
                    Currencies = _context.Currencies.Select(currency => new SelectListItem {Text = currency.StringCode, Value = currency.Id.ToString()}),
                    PriceFrom = 1,
                    Public = price.Publish,
                    SelCurr = price.Currency.Id,
                    Valuta = price.Currency.Code,
                    Currency = price.Currency
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

        public bool CheckPhotoToDelete(string fullPath)
        {
            var photoInUse = _context.ProductPhotos.Where(photo => photo.FullPath == fullPath).ToList();
            return photoInUse.Count <= 1;
        }

        public ProductPhoto GetProductPhoto(int id)
        {
            return _context.ProductPhotos.Find(id);
        }
        public IEnumerable<SelectListItem> GetValutes()
        {
            return
                _context.Currencies.Select(
                    currency => new SelectListItem {Text = currency.StringCode, Value = currency.Id.ToString()});
        }

        public IWorkbook GetPrice()
        {
            IWorkbook price = new XSSFWorkbook();
            ISheet sheet = price.CreateSheet(DateTime.Now.ToShortDateString());
            sheet.CreateRow(0).CreateCell(1).SetCellValue("Industrial Development");

            var style = price.CreateCellStyle();

            var font = price.CreateFont();
            font.FontHeightInPoints=24;
            style.SetFont(font);
            sheet.GetRow(0).GetCell(1).CellStyle=style;

            sheet.SetColumnWidth(0,7000);
            sheet.SetColumnWidth(1,25000);
            sheet.SetColumnWidth(2,3000);
            sheet.SetColumnWidth(3,3000);
            sheet.SetColumnWidth(4,3000);
            sheet.SetColumnWidth(5,3000);
            sheet.SetColumnWidth(6,5000);
            sheet.SetColumnWidth(7,3000);
            sheet.SetColumnWidth(8,3000);
            sheet.SetColumnWidth(9,3000);

            var cellMenuStyle = price.CreateCellStyle();
            cellMenuStyle.FillForegroundColor = IndexedColors.LightGreen.Index;
            cellMenuStyle.FillPattern = FillPattern.SolidForeground;
            cellMenuStyle.BorderBottom = BorderStyle.Thin;
            cellMenuStyle.BorderLeft = BorderStyle.Thin;
            cellMenuStyle.BorderRight = BorderStyle.Thin;
            cellMenuStyle.BorderTop = BorderStyle.Thin;

            var cellBorderProduct = price.CreateCellStyle();
            cellBorderProduct.BorderBottom = BorderStyle.Thin;
            cellBorderProduct.BorderLeft = BorderStyle.Thin;
            cellBorderProduct.BorderRight = BorderStyle.Thin;
            cellBorderProduct.BorderTop = BorderStyle.Thin;

            var cellSmenu = price.CreateCellStyle();
            cellSmenu.FillForegroundColor = IndexedColors.LightOrange.Index;
            cellSmenu.FillPattern = FillPattern.SolidForeground;


            var startRow = 2;
            var pCat = _context.ProductMenus.ToList();
            foreach (var menu in pCat)
            {
                var mRow = sheet.CreateRow(startRow);
                for (int i = 0; i < 11; i++)
                {
                    mRow.CreateCell(i);
                    mRow.GetCell(i).CellStyle = cellMenuStyle;
                }
                var mCell = mRow.CreateCell(0);
                mCell.SetCellValue(menu.Title);
                mCell.CellStyle = cellMenuStyle;
                sheet.AddMergedRegion(new CellRangeAddress(startRow, startRow, 0, 10));
                startRow++;
                foreach (var cat in menu.MenuItems)
                {
                    var sRow = sheet.CreateRow(startRow);
                    for (int i = 0; i < 11; i++)
                    {
                        sRow.CreateCell(i);
                        sRow.GetCell(i).CellStyle = cellSmenu;
                    }
                    sRow.GetCell(0).SetCellValue(cat.Title);
                    sheet.AddMergedRegion(new CellRangeAddress(startRow, startRow, 0, 10));
                    startRow++;
                    sRow = sheet.CreateRow(startRow);
                    sRow.CreateCell(0).CellStyle = cellBorderProduct;
                    sRow.GetCell(0).SetCellValue("Артикул");
                    sRow.CreateCell(1).CellStyle = cellBorderProduct;
                    sRow.GetCell(1).SetCellValue("Наименование");
                    sRow.CreateCell(2).CellStyle = cellBorderProduct;
                    sRow.GetCell(2).SetCellValue("Розница");
                    sRow.CreateCell(3).CellStyle = cellBorderProduct;
                    sRow.GetCell(3).SetCellValue("Опт");
                    sRow.CreateCell(4).CellStyle = cellBorderProduct;
                    sRow.GetCell(4).SetCellValue("Партнер");
                    sRow.CreateCell(5).CellStyle = cellBorderProduct;
                    sRow.GetCell(5).SetCellValue("Распродажа");
                    sRow.CreateCell(6).CellStyle = cellBorderProduct;
                    sRow.GetCell(6).SetCellValue("Брэнд");
                    sRow.CreateCell(7).CellStyle = cellBorderProduct;
                    sRow.GetCell(7).SetCellValue("Валюта");
                    sRow.CreateCell(8).CellStyle = cellBorderProduct;
                    sRow.GetCell(8).SetCellValue("Склад");
                    sRow.CreateCell(9).CellStyle = cellBorderProduct;
                    sRow.GetCell(9).SetCellValue("Ед.изм.");
                    sRow.CreateCell(10).CellStyle = cellBorderProduct;
                    sRow.GetCell(10).SetCellValue("Гарантия");
                    startRow++;
                    foreach (var product in cat.Products)
                    {
                        var rw = sheet.CreateRow(startRow);
                        rw.CreateCell(0).SetCellValue(product.Articul);
                        rw.GetCell(0).CellStyle = cellBorderProduct;
                        rw.CreateCell(1).SetCellValue(product.Title);
                        rw.GetCell(1).CellStyle = cellBorderProduct;
                        rw.GetCell(1).CellStyle.WrapText = true;
                        rw.CreateCell(2).SetCellValue((double)product.Prices.FirstOrDefault(p=>p.PriceType==PriceType.Retail)?.Value);
                        rw.GetCell(2).CellStyle = cellBorderProduct;
                        rw.CreateCell(3).SetCellValue((double)product.Prices.FirstOrDefault(p => p.PriceType == PriceType.LowOpt)?.Value);
                        rw.GetCell(3).CellStyle = cellBorderProduct;
                        rw.CreateCell(4).SetCellValue((double)product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Opt)?.Value);
                        rw.GetCell(4).CellStyle = cellBorderProduct;
                        rw.CreateCell(5).SetCellValue((double)product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Sale)?.Value);
                        rw.GetCell(5).CellStyle = cellBorderProduct;
                        rw.CreateCell(6).SetCellValue(product.Brand.FullName);
                        rw.GetCell(6).CellStyle = cellBorderProduct;
                        var curId = (int)product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Retail)?.Currency.Id;
                        rw.CreateCell(7).SetCellType(CellType.Numeric);
                        rw.GetCell(7).SetCellValue(curId);
                        rw.GetCell(7).CellStyle = cellBorderProduct;
                        rw.CreateCell(8).SetCellValue(product.Stock.Id);
                        rw.GetCell(8).CellStyle = cellBorderProduct;
                        rw.CreateCell(9).SetCellValue(product.MesureUnit.Id);
                        rw.GetCell(9).CellStyle = cellBorderProduct;
                        double war = int.Parse(product.Warranty);
                        rw.CreateCell(10).SetCellType(CellType.Numeric);
                        rw.GetCell(10).SetCellValue(war);
                        rw.GetCell(10).CellStyle = cellBorderProduct;
                        startRow++;
                    }
                }

            }
            return price;
        }

        public CustomerStatus GetCustomerStatus(int id)
        {
            return _context.Users.Find(id).Customer.CustomerStatus;
        }

        public string MakeProductsXml()
        {
            return Sitemap.MakeXmlList(_context.Products.ToList());
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders.OrderByDescending(order => order.OrderDate).ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders.Find(id);
        }

        public MenuVm SubMenuItems(int parent)
        {
            var wow = new MenuVm
            {
                Parent = _context.ProductMenuItems.Find(parent),
                Childs = _context.ProductMenuItems.Where(item => item.ParentMenuItem.Id==parent)
            };
            return wow;
        }
    }
}