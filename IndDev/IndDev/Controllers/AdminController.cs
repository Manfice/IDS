using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;
using IndDev.Models;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace IndDev.Controllers
{
    [Authorize(Roles = "A,M,O")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _repository;

        public AdminController(IAdminRepository repository)
        {
            _repository = repository;
        }


        // GET: Admin
        public ActionResult AdminPage(int userId)
        {
            var user = _repository.User(userId);
            return View(user);
        }

        public ActionResult MyProfile(int custId)
        {
            return View(_repository.Customer(custId));
        }

        public PartialViewResult AdminNavigation(string selCat = null)
        {
            ViewBag.Category = selCat;

            return PartialView();
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
            ViewBag.Roles = roles;
            return PartialView(_repository.User(id));
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
        public ActionResult ChangeUser(User user)
        {
            var result = _repository.UpdateUser(user);
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
            return PartialView(cat.MenuItems);
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
                ProductMenu = _repository.GetProductMenu(menuId)
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

        public ActionResult SubCatDetails(int id, string returnUrl)
        {
            var sc = _repository.GetSubCat(id);
            @ViewBag.ReturnUrl = returnUrl;
            @ViewBag.Title = $"Подкатегория {sc.Title}";
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
            return PartialView(_repository.GetrootMenuItems());
        }

        public PartialViewResult SubMenu(int id)
        {
            var b = _repository.GetSubMenuItems(id);
            return PartialView(b);
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
                photo.SaveAs(fullPath);
                var ava = _repository.GetProduct(id).ProductPhotos.FirstOrDefault(productPhoto => productPhoto.PhotoType == PhotoType.Avatar);
                if (ava != null)
                {
                    if (System.IO.File.Exists(ava.FullPath))
                    {
                        System.IO.File.Delete(ava.FullPath);
                    }
                    _repository.RemovePhoto(ava.Id);
                }
                var image = new ProductPhoto
                {
                    AltText = fileName,
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
        public ActionResult RemoveImage(int id)
        {
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
    }
}