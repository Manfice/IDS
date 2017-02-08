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
using IndDev.Filters;
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
        [Visitors]
        [Route("Catalog")] // canonical
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

        [Visitors]
        public ActionResult CatDetails(int catId)//int catId canonical
        {
            ///<summary> Заглушка для старых ссылок по яндексу. Надеюсь скоро он будет не нужен.</summary>
            var model = _repository.GetProductMenu(catId);
            ViewBag.Title = model.Title + @" Торговый дом ""АЙДИ-С""";
            ViewBag.Products = model.Id;
            ViewBag.canonical = $"catalog/{model.CanonicalTitle}";

            return RedirectToAction("CatDetails", new { canonic=model.CanonicalTitle});
        }

        [Visitors]
        [Route("catalog/{canonic}")]
        public ActionResult CatDetails(string canonic)//int catId canonical
        {
            var model = _repository.GetProductMenu(canonic);
            ViewBag.Title = model.Title+@" Торговый дом ""АЙДИ-С""";
            ViewBag.Products = model.Id;
            ViewBag.canonical = $"catalog/{canonic}";

            return View(model);
        }
        [Visitors]
        public ActionResult ShowProducts(int catId, Cart cart)
        {
            var model = _repository.GetProduct(catId);
            model.Cart = cart;
            ViewBag.Title = model.ProductMenuItem.Title+ @" Торговый дом ""АЙДИ-С"" в г. Ставрополе.";
            return RedirectToAction("ShowProducts", new { canonical=model.ProductMenuItem.CanonicalTitle});
        }

        [Visitors]
        [Route("Category/{canonical}")]
        public ActionResult ShowProducts(string canonical, Cart cart)
        {
            var model = _repository.GetProduct(canonical);
            model.Cart = cart;
            ViewBag.Title = model.ProductMenuItem.Title + @" Торговый дом ""АЙДИ-С"" в г. Ставрополе.";
            ViewBag.canonical = $"Category/{canonical}";
            return View(model);
        }

        [Route("Baraholochka")]
        public ActionResult ShowOffers(Cart cart) //canonic
        {
            var model = _repository.GetTopRetails;
            ViewBag.Cart = cart;
            ViewBag.Title = @"Наша ""Барахолочка""";
            ViewBag.canonical = "Baraholochka";

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
                item.Id, item.Title, item.Products.Count, item.Image.Path, products = item.Products.Where(product => product.Reclama).Select(product =>
                {
                    var sale = product.Prices.FirstOrDefault(price => price.PriceType==PriceType.Sale);
                    var ret = product.Prices.FirstOrDefault(price => price.PriceType == PriceType.Retail);
                    return sale != null && sale.Value>0 ? new ProductAjax{
                        Price = sale.GetPriceRubl(),Title = product.Title, Id = product.Id, Avatar = product.ProductPhotos.FirstOrDefault(a=>a.PhotoType==PhotoType.Avatar).Path, Rate = product.Rate, Art = product.Articul
                    } : new ProductAjax
                    {
                        Price = ret.GetPriceRubl(),
                        Title = product.Title,
                        Id = product.Id,
                        Avatar = product.ProductPhotos.FirstOrDefault(a => a.PhotoType == PhotoType.Avatar).Path,
                        Rate = product.Rate,
                        Art = product.Articul
                    };
                })
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }  
    }
}