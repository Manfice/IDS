using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Cart;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly ICartRepository _repository;
        private int _user;

        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }
        protected override void Initialize(RequestContext context)
        {
            base.Initialize(context);
            if (!context.HttpContext.User.Identity.IsAuthenticated) return;
            _user = int.Parse(context.HttpContext.User.Identity.Name);
        }
        // GET: Cart
        public ActionResult Index(Cart cart)
        {
            if (_user>0)
            {
                cart.Customer = _repository.GetCustomer(_user);
            }
            return View(cart);
        }

        public PartialViewResult CartModule(Cart cart)
        {
            return PartialView(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int productId, int quantity, Cart cart, string returnUrl)
        {
            var product = _repository.GetProduct(productId);
            if (product == null) return View("Index");
            cart.AddItem(product,quantity);
            return Redirect(returnUrl);
        }

        public ActionResult QuickAddToCart(int prodId, string returnUrl, Cart cart)
        {
            var product = _repository.GetProduct(prodId);
            var customer = _repository.GetCustomer(_user);
            cart.AddItem(product,1);
            return Redirect(returnUrl);
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