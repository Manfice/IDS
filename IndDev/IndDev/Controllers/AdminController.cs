using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace IndDev.Controllers
{
    [Authorize(Roles = "A,M,O")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _repository;
        private int _user;

        public AdminController(IAdminRepository repository)
        {
            _repository = repository;
        }
        protected override void Initialize(RequestContext context)
        {
            base.Initialize(context);
            if (!context.HttpContext.User.Identity.IsAuthenticated) return;
            _user = int.Parse(context.HttpContext.User.Identity.Name);
        }
        // GET: Admin
        public ActionResult AdminPage(int userId)
        {
            var user = _repository.User(userId);
            return View(user);
        }

        public PartialViewResult ShowSm()
        {
            ViewBag.Mystr = _repository.MakeProductsXml();
            return PartialView();
        }
        public ActionResult MyProfile(int custId)
        {
            return View(_repository.Customer(custId));
        }

        public PartialViewResult AdminNavigation(string selCat = null)
        {
            ViewBag.Category = selCat;
            var model = _repository.User(_user);
            return PartialView(model);
        }

        public ActionResult Orders()
        {
            var model = _repository.GetOrders();
            return View(model);
        }

        public ActionResult OrderDetails(int id)
        {
            return View(_repository.GetOrderById(id));
        }
        public ActionResult UserList()
        {
            return View();
        }

        public PartialViewResult UsersList()
        {
            return PartialView(_repository.Users());
        }

        public ActionResult EditUser(int id)
        {
            var user = _repository.User(id);
            ViewBag.Title = user.Name;
            return View(user);
        }

        public ActionResult DeleteUsr(int id)
        {
            return RedirectToAction("UserList");
        }

        public PartialViewResult UserSummary(int userId)
        {
            var model = _repository.User(userId);
            ViewBag.Title = model.Name;
            return PartialView(model);
        }

        public PartialViewResult ChangeUser(int id)
        {
            var roles = new SelectList(_repository.Roleses, "Id", "Descr");
            var ststus = _repository.GetCustomerStatuses.Select(status => new SelectListItem
            {
                Text = status.Title+" скидка:"+status.Discount+"%",
                Value = status.Id.ToString()
            });
            var model = new UserViewModel
            {
                User = _repository.User(id),
                Roles = _repository.Roleses.Select(usrRoles => new SelectListItem
                {
                    Text = usrRoles.Descr,
                    Value = usrRoles.Id.ToString()
                }),
                Status = ststus
            };
            ViewBag.Rol = roles;
            return PartialView(model);
        }

        public ActionResult Products()
        {
            return View();
        }

        public PartialViewResult ProductIndex()
        {
            return PartialView();
        }

        public PartialViewResult Categories()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUser(UserViewModel model)
        {
            var result = _repository.UpdateUser(model.User);
            if (result.Code < 0)
            {
                ViewBag.ReturnUrl = "//Admin/UserList";
                return View("Error");
            }
            TempData["valid"] = result.Message;
            return RedirectToAction("EditUser", new {id = result.Code});
        }

        public PartialViewResult CategoriesList()
        {
            return PartialView(_repository.ProductMenus().OrderBy(menu => menu.Priority));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProdCat(ProductMenu productMenu, HttpPostedFileBase photo)
        {
            var pMenu = new ProductMenu
            {
                Title = productMenu.Title,
                Descr = productMenu.Descr,
                Priority = productMenu.Priority
            };

            if (photo != null)
            {
                var fileName = photo.FileName;
                var filePath = Server.MapPath("/Content/images/Uploads/Categories");
                var fullPath = Path.Combine(filePath, fileName);
                photo.SaveAs(fullPath);
                pMenu.Image = new ProdMenuImage
                {
                    AltText = fileName,
                    FullPath = fullPath,
                    Path = "/Content/images/Uploads/Categories/" + fileName
                };
            }
            var result = _repository.AddCategory(pMenu);

            return RedirectToAction("CatDetails", _repository.GetProductMenu(result.Code));
        }

        public ActionResult DeleteCat(int id)
        {
            TempData["message"] = _repository.RemoveCategory(id).Messge;
            return RedirectToAction("Products");
        }

        public ActionResult CatDetails(int id)
        {
            return View(_repository.GetProductMenu(id));
        }

        public ActionResult EditCategory(ProductMenu menu, HttpPostedFileBase photo)
        {
            ValidEvent result;
            if (photo != null)
            {
                var fileName = photo.FileName;
                var filePath = Server.MapPath("/Content/images/Uploads/Categories");
                var fullPath = Path.Combine(filePath, fileName);
                photo.SaveAs(fullPath);
                var image = new ProdMenuImage
                {
                    AltText = fileName,
                    FullPath = fullPath,
                    Path = "/Content/images/Uploads/Categories/" + fileName
                };
                result = _repository.UpdateCategory(menu, image);
            }
            else
            {
                result = _repository.UpdateCategory(menu, null);
            }

            return RedirectToAction("CatDetails", new {id = result.Code});
        }

        public PartialViewResult SubCutList(int id)
        {
            var cat = _repository.GetProductMenu(id);
            return PartialView(cat.MenuItems.Where(item => item.ParentMenuItem==null).OrderBy(item => item.Priority));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubCut(ProductMenuItem item, HttpPostedFileBase photo, int menuId)
        {
            var pMenu = new ProductMenuItem
            {
                Title = item.Title,
                Descr = item.Descr,
                IsRus = item.IsRus,
                ProductMenu = _repository.GetProductMenu(menuId),
                Priority = item.Priority
            };

            if (photo != null)
            {
                var fileName = photo.FileName;
                var filePath = Server.MapPath("/Content/images/Uploads/Categories");
                var fullPath = Path.Combine(filePath, fileName);
                photo.SaveAs(fullPath);
                pMenu.Image = new ProdMenuImage
                {
                    AltText = fileName,
                    FullPath = fullPath,
                    Path = "/Content/images/Uploads/Categories/" + fileName
                };
            }
            _repository.AddSubCategory(pMenu);
            return RedirectToAction("CatDetails", new {id = menuId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProducts(int catId,HttpPostedFileBase prodList)
        {
            if (prodList.ContentLength>0)
            {
                if (prodList.FileName.EndsWith("xls")|| prodList.FileName.EndsWith("xlsx"))
                {
                    var path = Server.MapPath("~/Upload/" + prodList.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    prodList.SaveAs(path);
                    var pList = new List<ProductExcell>();
                    using (var fs = System.IO.File.OpenRead(path))
                    {
                        var wb = new XSSFWorkbook(fs);
                        var wsh = wb.GetSheetAt(0);
                        for (var row = 1; row < wsh.LastRowNum+1; row++)
                        {
                            var p = new ProductExcell();
                            p.Art = wsh.GetRow(row).GetCell(0).StringCellValue;
                            p.Title= wsh.GetRow(row).GetCell(1).StringCellValue;
                            p.Retail = (decimal)wsh.GetRow(row).GetCell(2).NumericCellValue;
                            p.Opt = (decimal)wsh.GetRow(row).GetCell(3).NumericCellValue;
                            p.Partner = (decimal)wsh.GetRow(row).GetCell(4).NumericCellValue;
                            p.Sale = (decimal)wsh.GetRow(row).GetCell(5).NumericCellValue;
                            p.Brand = wsh.GetRow(row).GetCell(6).StringCellValue;
                            p.Curr = (int)wsh.GetRow(row).GetCell(7).NumericCellValue;
                            p.Stock = (int) wsh.GetRow(row).GetCell(8).NumericCellValue;
                            p.MeasureUnit = (int) wsh.GetRow(row).GetCell(9).NumericCellValue;
                            p.Warranty = (int)wsh.GetRow(row).GetCell(10).NumericCellValue;
                            pList.Add(p);
                        }
                    }
                    _repository.UpToDateProducts(pList,catId);
                }
            }
            return RedirectToAction("SubCatDetails",new {id=catId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateByPrice(HttpPostedFileBase price)
        {
            if (price.ContentLength <= 0) return RedirectToAction("Products");
            if (price.FileName.EndsWith("xls")||price.FileName.EndsWith("xlsx"))
            {
                var path = Server.MapPath("~/Upload/" + price.FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                price.SaveAs(path);
                var pList = new List<ProductExcell>();
                using (var pL = System.IO.File.OpenRead(path))
                {
                    var wb = new XSSFWorkbook(pL);
                    var sheet = wb.GetSheetAt(0);
                    for (var row = 0; row < sheet.LastRowNum+1; row++)
                    {
                        var currRow = sheet.GetRow(row);
                        if (IsProduct(currRow))
                        {
                            var p = new ProductExcell();
                            p.Art = currRow.GetCell(0).StringCellValue;
                            p.Title = currRow.GetCell(1).StringCellValue;
                            p.Retail = (decimal)currRow.GetCell(2).NumericCellValue;
                            p.Opt = (decimal)currRow.GetCell(3).NumericCellValue;
                            p.Partner = (decimal)currRow.GetCell(4).NumericCellValue;
                            p.Sale = (decimal)currRow.GetCell(5).NumericCellValue;
                            p.Brand = currRow.GetCell(6).StringCellValue;
                            p.Curr = int.Parse(currRow.GetCell(7).StringCellValue);
                            p.Stock = (int)currRow.GetCell(8).NumericCellValue;
                            p.MeasureUnit = (int)currRow.GetCell(9).NumericCellValue;
                            p.Warranty = (int) currRow.GetCell(10).NumericCellValue;
                            pList.Add(p);
                        }
                    }
                }
                _repository.UpdateByPrice(pList);
            }
            return RedirectToAction("Products");
        }

        private bool IsProduct(IRow row)
        {
            var result = false;
            if (row==null)
            {
                return false;
            }
            if (row.GetCell(0)!=null && !string.IsNullOrWhiteSpace(row.GetCell(0).StringCellValue))
            {
                if (row.GetCell(0).StringCellValue == "Артикул") return false;
                if (!string.IsNullOrWhiteSpace(row.GetCell(1).StringCellValue))
                {
                    result = true;
                }
            }
            return result;
        }
        public ActionResult GetPriceList()
        {
            var doc = _repository.GetPrice();
            var path = Server.MapPath("~/Content/report.xlsx");
            var report = System.IO.File.Create(path);
            doc.Write(report);
            report.Close();
            var result = System.IO.File.OpenRead(path);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "price.xlsx");
        }

        public ActionResult SubCatDetails(int id, string returnUrl)
        {

            var sc = _repository.SubMenuItems(id);
            @ViewBag.ReturnUrl = returnUrl;
            @ViewBag.Title = $"Подкатегория {sc.Parent.Title}";
            return View(sc);
        }

        public ActionResult EditSubCategory(ProductMenuItem menu, HttpPostedFileBase photo, string retUrl)
        {
            ValidEvent result;
            if (photo != null)
            {
                var fileName = photo.FileName;
                var filePath = Server.MapPath("/Content/images/Uploads/Categories");
                var fullPath = Path.Combine(filePath, fileName);
                photo.SaveAs(fullPath);
                var image = new ProdMenuImage
                {
                    AltText = fileName,
                    FullPath = fullPath,
                    Path = "/Content/images/Uploads/Categories/" + fileName
                };
                result = _repository.UpdateSubCategory(menu, image);
            }
            else
            {
                result = _repository.UpdateSubCategory(menu, null);
            }

            return RedirectToAction("SubCatDetails", new {id = result.Code, returnUrl = retUrl});
        }

        public ActionResult RemoveSubCat(int id, int root)
        {
            TempData["message"] = _repository.RemoveSubCat(id).Messge;
            return RedirectToAction("CatDetails", new {id = root});
        }

        public PartialViewResult RootMenu()
        {
            return PartialView(_repository.ProductMenus());
        }
        public ActionResult SubMenu(int id)
        {
            var b = _repository.SubMenuItems(id);
            return View(b);
        }

        public PartialViewResult AddSubMenu(int parent)
        {
            return PartialView(_repository.GetMenu(parent));
        }

        public ActionResult AddMenuItem(Menu model)
        {
            _repository.AddSubMenuItem(model);
            return RedirectToAction("Products");
        }

        public ActionResult RemoveSubMenu(int id)
        {
            _repository.RemoveSubMenu(id);
            return RedirectToAction("Products");
        }

        public PartialViewResult ProductList(int menuId)
        {
            var p = _repository.GetProductsByMenu(menuId);
            return PartialView(p);
        }

        public PartialViewResult AddProductToSubCat(int subCatId, string returnUrl)
        {
            var p = new AddProductViewModel
            {
                SubCatId = subCatId,
                ReturnUrl = returnUrl,
                Brands = _repository.GetAllBrands(),
                MesureUnits = _repository.GetAllMesureUnits(),
                Stocks = _repository.GetStocks,
                Product = new Product()
            };

            return PartialView(p);
        }

        public PartialViewResult ShowProducts(int subCatId)
        {
            var p = _repository.GetCatProduct(subCatId);
            var pvm = new List<ProductViewModel>();
            foreach (var item in p)
            {
                var pvmItem = new ProductViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Currency = item.Prices.FirstOrDefault(price => price.PriceType==PriceType.LowOpt)?.Currency.Code,
                    Articul = item.Articul,
                    PriceIn = item.Prices.FirstOrDefault(price => price.PriceType==PriceType.LowOpt).Value,
                    PriceOut = item.Prices.FirstOrDefault(price => price.PriceType == PriceType.Retail).Value
                };
                pvm.Add(pvmItem);
            }
            return PartialView(p);
        }

        public ActionResult ProductDetails(int id)
        {
            var product = _repository.GetProduct(id);
            var prodAva = product.ProductPhotos?.FirstOrDefault(photo => photo.PhotoType == PhotoType.Avatar);
            var pdvm = new ProductDetailsViewModel
            {
                Product = product,
                Brands = _repository.GetAllBrands().Select(brand=>new SelectListItem { Text = brand.FullName, Value = brand.Id.ToString()}),
                Vendors = _repository.GetAllVendors().Select(vendor=>new SelectListItem {Text = vendor.Title,Value = vendor.Id.ToString()}),
                MesureUnits = _repository.GetAllMesureUnits().Select(mu=>new SelectListItem {Text = mu.FullName, Value = mu.Id.ToString()}),
                Stocks = _repository.GetStocks.Select(stock => new SelectListItem {Value = stock.Id.ToString(),Text = stock.Title}),
                Avatar = prodAva,
                Prices = _repository.GetPrices(product.Id),
                SelBr = product.Brand.Id,
                SelVr = product.Vendor.Id,
                SelMu = product.MesureUnit.Id,
                SelStock = product.Stock.Id
            };
            ViewBag.Title = product.Title;
            return View(pdvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(ProductDetailsViewModel model)
        {
            var p = _repository.UpdateProduct(model);
            return RedirectToAction("ProductDetails", new {id=p.Id});
        }

        public ActionResult AddAvatar(int id, HttpPostedFileBase photo)
        {
            if (photo!=null)
            {
                var fileName = Guid.NewGuid()+"_"+photo.FileName;
                var filePath = Server.MapPath("/Content/images/Uploads/Categories");
                var fullPath = Path.Combine(filePath, fileName);
                var altText = fileName;
                photo.SaveAs(fullPath);
                var ava = _repository.GetProduct(id).ProductPhotos.FirstOrDefault(productPhoto => productPhoto.PhotoType == PhotoType.Avatar);
                if (ava != null)
                {
                    if (System.IO.File.Exists(ava.FullPath))
                    {
                        if (_repository.CheckPhotoToDelete(ava.FullPath))
                        {
                            System.IO.File.Delete(ava.FullPath);
                        }
                    }
                    _repository.RemovePhoto(ava.Id);
                }
                var image = new ProductPhoto
                {
                    AltText = altText,
                    FullPath = fullPath,
                    Path = "/Content/images/Uploads/Categories/" + fileName,
                    PhotoType = PhotoType.Avatar
                };
                var result = _repository.AddPhotoToProduct(id, image);
            }
            return RedirectToAction("ProductDetails", new { id });
        }
        public ActionResult AddPhoto(int id, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                var fileName = Guid.NewGuid() + "_" + photo.FileName;
                var filePath = Server.MapPath("/Content/images/Uploads/Categories");
                var fullPath = Path.Combine(filePath, fileName);
                photo.SaveAs(fullPath);
                var image = new ProductPhoto
                {
                    AltText = fileName,
                    FullPath = fullPath,
                    Path = "/Content/images/Uploads/Categories/" + fileName,
                    PhotoType = PhotoType.Photo
                };
                var result = _repository.AddPhotoToProduct(id, image);
            }
            return RedirectToAction("ProductDetails", new {id});
        }
        public ActionResult RemoveImage(int id)//id of image in ProductPhoto table
        {
            var pp = _repository.GetProductPhoto(id);
            if (System.IO.File.Exists(pp.FullPath))
            {
                if (_repository.CheckPhotoToDelete(pp.FullPath))
                {
                    System.IO.File.Delete(pp.FullPath);
                }
            }
            var result = _repository.RemoveImage(id);
            return RedirectToAction("ProductDetails", new {id=result.Code});
        }
        public PartialViewResult SetPriceSection(int pId)
        {
            var p = _repository.GetProduct(pId);
            return PartialView(p);
        }

        public PartialViewResult PriceForProd(int priceId)
        {
            var ps = _repository.GetPriceSetter(priceId);
            return PartialView(ps);
        }
        [HttpPost]
        public ActionResult SavePrice(PriceSetter model)
        {
            var result = _repository.SetPrice(model);
            return RedirectToAction("ProductDetails", new {id=result.Code});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(AddProductViewModel model)
        {
            _repository.SaveProduct(model);
            return RedirectToAction("SubCatDetails", new {id = model.SubCatId, returnUrl = model.ReturnUrl});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhotoToProduct(AddPhotoViewModel model)
        {
            if (model.Photo!=null)
            {
                var fileName = model.Photo.FileName;
                var filePath = Server.MapPath("/Content/images/Uploads/Categories");
                var fullPath = Path.Combine(filePath, fileName);
                model.Photo.SaveAs(fullPath);
                var image = new ProductPhoto
                {
                    
                    AltText = fileName,
                    FullPath = fullPath,
                    Path = "/Content/images/Uploads/Categories/" + fileName
                };
                var result = _repository.AddPhotoToProduct(model.ProductId, image);
            }
            return RedirectToAction("ProductDetails", new {id = model.ProductId});
        }

        public ActionResult RemoveProduct(int id, string returnUrl)
        {
            var result = _repository.RemoveProduct(id);
            return RedirectToAction("SubCatDetails", new {id=result.Code, returnUrl=returnUrl});
        }

        public ActionResult CustomersList()
        {
            return View(_repository.GetCustomers);
        }

        public ActionResult CustomerData(int cust)
        {
            ViewBag.Title = "Данные клиента";
            var customer = _repository.Customer(cust);
            var custStat = _repository.GetCustomerStatuses.Select(status => new SelectListItem
            {
                Value = status.Id.ToString(),
                Text = status.Title
            });
            var model = new EditCustomer
            {
                Id = customer.Id,
                Title = customer.Title,
                Adress = customer.Adress,
                Register = customer.Register,
                Status = customer.CustomerStatus.Id,
                CustStatus = custStat,
                Details = customer.Details
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCustomerData(EditCustomer model)
        {
            var result = _repository.SaveCustomer(model);
            return RedirectToAction("CustomerData", new {cust = result.Id});
        }

        public ActionResult AddReqSubCat(int parent, string returnUrl)
        {
            var parentCategory = _repository.GetSubCat(parent);
            ViewBag.ru = returnUrl;
            return PartialView(parentCategory);
        }

        public ActionResult SubSubCatList(int subcut)
        {
            var model = _repository.SubMenuItems(subcut);
            return PartialView(model);
        }
    }
}