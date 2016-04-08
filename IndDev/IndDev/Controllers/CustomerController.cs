using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.ViewModels;

namespace IndDev.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomer _repository;
        private int _user;

        public CustomerController(ICustomer repo)
        {
            _repository = repo;
        }
        protected override void Initialize(RequestContext context)
        {
            base.Initialize(context);
            if (!context.HttpContext.User.Identity.IsAuthenticated) return;
            _user = int.Parse(context.HttpContext.User.Identity.Name);
        }
        // GET: Customer
        public ActionResult Index()
        {
            var customer = _repository.GetCustomerByUserId(_user);
            return View(customer);
        }

        public PartialViewResult LkNavigation(string select)
        {
            var nav = new List<CustMenuItems>
            {
                new CustMenuItems {Title = "Личные данные", MenuLink = "/Customer/Index"},
                new CustMenuItems {Title = "Заказы", MenuLink = "/Customer/Index"},
                new CustMenuItems {Title = "Сальдо", MenuLink = "/Customer/Index"}
            };
            return PartialView(nav);
        }

        public PartialViewResult About()
        {
            var customer = _repository.GetCustomerByUserId(_user);
            return PartialView(customer);
        }
        public ActionResult CartCustomerView()
        {
            var cust = _repository.GetCustomerByUserId(_user);
            return PartialView(cust);
        }
    }
}