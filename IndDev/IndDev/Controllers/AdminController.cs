using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Menu;
using IndDev.Models;

namespace IndDev.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _repository;

        public AdminController(IAdminRepository repository)
        {
            _repository = repository;
        }

        [Authorize(Roles = "A,M,O")]

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
            return View(user);
        }

        public PartialViewResult UserSummary(int userId)
        {
            return PartialView(_repository.User(userId));
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
            _repository.AddCategory(pMenu);

            return RedirectToAction("ProductIndex");
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

            return RedirectToAction("CatDetails", new {id=result.Code});
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
            return RedirectToAction("CatDetails",new {id=menuId});
        }

        public ActionResult SubCatDetails(int id, string returnUrl)
        {
            var sc = _repository.GetSubCat(id);
            @ViewBag.ReturnUrl = returnUrl;
            @ViewBag.Title =$"Подкатегория {sc.Title}";
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

            return RedirectToAction("SubCatDetails", new { id = result.Code, returnUrl=retUrl });
        }

        public ActionResult RemoveSubCat(int id, int root)
        {
            TempData["message"] = _repository.RemoveSubCat(id).Messge;
            return RedirectToAction("CatDetails", new {id=root});
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
    }
}