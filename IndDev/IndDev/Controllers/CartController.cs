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
        private ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult CartModule(Cart cart)
        {
            return PartialView(cart);
        }
    }
}