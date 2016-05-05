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
            return PartialView(pvm);
        }

        public ActionResult ProductDetails(int id)
        {
            var product = _repository.GetProduct(id);
            var prodAva = product.ProductPhotos?.FirstOrDefault(photo => photo.PhotoType == PhotoType.Avatar);
            var pdvm = new ProductDetailsViewModel
            {
                Product = product,
                Brands = _repository.GetAllBrands(),
                Vendors = _repository.GetAllVendors(),
                MesureUnits = _repository.GetAllMesureUnits(),
                Avatar = prodAva
            };
            return View(pdvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(ProductDetailsViewModel model)
        {
            var prod = model.Product;
            var p = _repository.UpdateProduct(prod);
            return RedirectToAction("ProductDetails", new {id=p.Id});
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