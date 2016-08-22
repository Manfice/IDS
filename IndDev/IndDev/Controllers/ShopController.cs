using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Search(IEnumerable<ProductView> products, string searchString)
        {
            ViewBag.Search = searchString;
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string search)
        {

            var sProd = (from item in _repository.GetProducts
                         let pr = (item.Articul + item.Title).ToLowerInvariant()
                         where pr.Contains(search)
                         select new ProductView
                         {
                             Product = item,
                             Avatar = item.ProductPhotos.FirstOrDefault(photo => photo.PhotoType == PhotoType.Avatar)
                         }).ToList();

            var model = new SearchRequests
            {
                SearchReq = search,
                SearchResult = sProd.Capacity.ToString()
            };
            _repository.SaveSearch(model);

            return RedirectToAction("Search",new {products=sProd, searchString=search });
        }

        public ActionResult Calculator()
        {
            return PartialView();
        }
    }
}