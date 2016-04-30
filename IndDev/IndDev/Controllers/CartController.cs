using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Cart;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }
        // GET: Cart
        public ActionResult Index(Cart cart)
        {
            if (!User.Identity.IsAuthenticated) cart.Discount = 0;
            return View(cart);
        }

        public PartialViewResult CartModule(Cart cart)
        {
            return PartialView(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int productId, int quantity, Cart cart, int? sCat)
        {
            var product = _repository.GetProduct(productId);
            if (product == null) return View("Index");
            cart.AddItem(product,quantity);
            return RedirectToAction("CatDetails","Shop",new {catId = sCat, selCat = product.Categoy.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReCalc(int prodId, int quantity,Cart cart)
        {
            var product = _repository.GetProduct(prodId);
            if (product == null) return View("Index");
            cart.ChangeQuantity(product, quantity);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveItem(int prodId, Cart cart)
        {
            var product = _repository.GetProduct(prodId);
            if (product == null) return View("Index");
            cart.RemoveLine(product);
            return RedirectToAction("Index");
        }
        public ActionResult EmptyCart(Cart cart)
        {
            cart.ClearList();
            return RedirectToAction("Index");
        }
    }
}