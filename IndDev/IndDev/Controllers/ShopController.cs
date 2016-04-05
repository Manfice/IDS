using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Cart;
using IndDev.Domain.Entity.Menu;
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
            var catList = _repository.GetProductMenus();
            return View(catList);
        }

        public PartialViewResult Navigation()
        {
            var m = _repository.GetTopMenus();
            return PartialView(m);
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

        public ActionResult CatDetails(int catId, int selCat)
        {
            var model = _repository.GetProductMenu(catId);
            ViewBag.Title = model.Title;
            ViewBag.Products = selCat;
            return View(model);
        }

        public PartialViewResult ShowProducts(int catId, string subCat)
        {
            var sCatId = 0; int.TryParse(subCat, out sCatId);
            var model = _repository.GetProducts(catId, sCatId);
            return PartialView(model);
        }

        public PartialViewResult ProductView(int id, int subCat=0)
        {
            ViewBag.SubCat = subCat;
            return PartialView(_repository.GetProduct(id, subCat));
        }
    }
}