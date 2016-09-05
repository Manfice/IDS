using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Cart;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Orders;
using IndDev.Domain.ViewModels;

namespace IndDev.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomer _repository;
        private readonly IMailRepository _mail;
        private int _user;

        public CustomerController(ICustomer repo, IMailRepository mail)
        {
            _repository = repo;
            _mail = mail;
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
                new CustMenuItems {Title = "Сводка", MenuLink = "/Customer/Index"},
                new CustMenuItems {Title = "Личные данные", MenuLink = "/Customer/CustomerDetails"},
                new CustMenuItems {Title = "Заказы", MenuLink = "/Customer/Orders"},
                //new CustMenuItems {Title = "Сальдо", MenuLink = "/Customer/Index"}
            };
            return PartialView(nav);
        }

        public ActionResult Orders()
        {
            var model = _repository.GetOrders(_user);
            return View(model);
        }

        public ActionResult OrderDetails(int id)
        {
            var cust = _repository.GetCustomerByUserId(_user).Id;
            var model = _repository.GetOrderById(id, cust);
            if (model!=null)
            {
                return View(model);
            }
            return RedirectToAction("MessageScreen", "Home",
                new { message = "Заказ не найден. Попробуйте еще раз.", paragraf = "Ошибка поиска заказа!" });
        }
        public PartialViewResult About()
        {
            var customer = _repository.GetUserById(_user);
            return PartialView(customer);
        }
        public ActionResult CartCustomerView()
        {
            var cust = _repository.GetUserById(_user);
            return PartialView(cust);
        }

        public ActionResult CustomerDetails()
        {
            var user = _repository.GetUserById(_user);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCustomerData(User model)
        {
            _repository.UpdateCustomer(model);
            return RedirectToAction("Index");
        }

        public ActionResult MakeOrder(Cart cart)
        {
            var total = cart.CalcActualTotalWithDiscount();
            var customer = _repository.GetCustomerByUserId(_user);
            var delTypes = _repository.GetDeliveryTypes().Select(type => new SelectListItem
            {
                Text = (total>20000)? type.Title+" - БЕСПЛАТНО!" : type.Title + " - " + type.Cost.ToString("C"),
                Value = type.Id.ToString()
            });
            var preOrder = new PreOrder
            {
                Customer = customer,
                Delivery = new Delivery {Recipient = customer.Title, To = customer.Adress},
                DeliveryTypes = delTypes,
                DeliveryTypeCode = 5
            };
            return View(preOrder);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MakeOrder(Cart cart, PreOrder preorder)
        {
            if (!cart.CartItems.Any())
                return RedirectToAction("MessageScreen", "Home",
                    new { message = "Не выбран ни один товар. Ваша корзина пуста." });
            var order = _repository.MakeOrder(_user, cart, preorder);
            if (order == null)
                return RedirectToAction("MessageScreen", "Home",
                    new { message = "Заказ не сохранен. Попробуйте еще раз.", paragraf= "Ошибка записи заказа!" });

            cart.ClearList();
            await _mail.OrderNotify(order, "");
            return RedirectToAction("MyOrder",new {id=order.Id});
        }

        public ActionResult MyOrder(int id)
        {
            var cust = _repository.GetCustomerByUserId(_user).Id;
            var model = _repository.GetOrderById(id, cust);
            if (model != null)
            {
                return View(_repository.GetOrderById(id, cust));
            }
            return RedirectToAction("MessageScreen", "Home",
                new { message = "Заказ не найден. Попробуйте еще раз.", paragraf = "Ошибка поиска заказа!" });
        }
        public PartialViewResult Logon()
        {
            var customer = _repository.GetCustomerByUserId(_user);
            return PartialView(customer);
        }
    }
}