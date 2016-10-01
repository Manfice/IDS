using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Cart;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;
using IndDev.Models;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly IShopRepository _repository;

        public ShopController(IShopRepository repository)
        {
            _repository = repository;
        }
        // GET: Shop
        public ActionResult Index()
        {
            var catList = _repository.GetProductMenus;
            return View(catList);
        }

        public PartialViewResult Navigation(string selected="0",string product="0")
        {
            var catList = _repository.GetProductMenus;
            ViewBag.Selected = selected;
            ViewBag.Product = product;
            return PartialView(catList);
        }

        public ActionResult ShopBlock(Cart cart)
        {
            return PartialView(cart);
        }
        public PartialViewResult SubMenu(string id) //Curent menu item 
        {
            var curId = 0;
            var sm = new List<Menu>();
            if (int.TryParse(id,out curId))
            {
                sm = _repository.GetSubMenu(curId).ToList();
            }
            return PartialView(sm);
        }

        public ActionResult CatDetails(int catId)
        {
            var model = _repository.GetProductMenu(catId);
            ViewBag.Title = model.Title+@" Торговый дом ""АЙДИ-С""";
            ViewBag.Products = model.Id;
            return View(model);
        }

        public ActionResult ShowProducts(int catId, Cart cart)
        {
            var model = _repository.GetProduct(catId);
            model.Cart = cart;
            ViewBag.Title = model.ProductMenuItem.Title+ @"Торговый дом ""АЙДИ-С"" в г. Ставрополе.";
            return View(model);
        }
        public ActionResult ShowOffers(Cart cart)
        {
            var model = _repository.GetTopRetails;
            ViewBag.Cart = cart;
            ViewBag.Title = @"Самые сильные предложения от " + @"Торговый дом ""АЙДИ-С"" в г. Ставрополе.";
            return View(model);
        }
        public ActionResult ProductDetails(Cart cart, int id)
        {
            var model = _repository.GetProductDetails(id);
            model.Cart = cart;
            return View(model);
        }

        public PartialViewResult ProductView(int id, int subCat=0)
        {
            ViewBag.SubCat = subCat;
            return PartialView(_repository.GetProduct(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search (Cart cart,SearchModel resultModel)
        {
            var model = await _repository.SearchProductsAsynk(resultModel.SearchRequest);
            model.Cart = cart;
            ViewBag.SearchRequest = resultModel.SearchRequest;
            return View(model);
        }

        public ActionResult IndexConv()
        {
            ViewBag.Title = "Каталог товаров компании Торговый дом \"АЙДИ-С\"";
            return View();
        }

        public async Task<JsonResult> GetTopMenus()
        {
            if (!Request.IsAjaxRequest()) return null;
            var model = await _repository.GetTopMenus();
            var result = model.Select(c => new
            {
                c.Id, c.Title,
                Picture = c.Image.Path, c.Priority
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetTopRetails()
        {
            if (!Request.IsAjaxRequest()) return null;
            var i = await _repository.GetRetails();
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetBrandsPic()
        {
            var model = await _repository.GetBrandsPicAsync();
            var dm = model.Select(m => m.BrandImage).Distinct();
            return Json(dm, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCategorys(int id)
        {
            var model = await _repository.GetMenuItems(id);
            var result = model.Where(item => item.Image!=null).Select(item => new
            {
                item.Id, item.Title, item.Products.Count, item.Image.Path
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }  
    }
}