using System;
using System.Security.Cryptography;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Context;
using IndDev.Models;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ISecureRepository _repository;
        private readonly IHomeRepository _home;

        public HomeController(ISecureRepository repository, IHomeRepository home)
        {
            _repository = repository;
            _home = home;
        }
        
        public ActionResult Index()
        {
            ViewBag.Title = "Industrial Development";
            return View();
        }

        //public ActionResult Test()
        //{
        //    ViewBag.Title = "Industrial Development";
        //    return View();
        //}
        public ActionResult About()
        {
            return View();
        }
        
        public ActionResult Contact()
        {
            return View();
        }
        //public ActionResult Landing()
        //{
        //    return View();
        //}

        public PartialViewResult Login(int id)
        {
            var usr = _repository.GetUserById(id);
            return PartialView(usr);
        }

        public PartialViewResult Currensy()
        {
            var cur = _home.GetCurses(DateTime.Today);
            return PartialView(cur);
        }

        public ActionResult TopNavigation()
        {
            return PartialView();
        }
        public PartialViewResult CurrensyNew()
        {
            var cur = _home.GetCurses(DateTime.Today);
            return PartialView(cur);
        }

        public ActionResult FeedBack()
        {
            return PartialView(new MailMessageModel());
        }
        public ActionResult MessageScreen(string message, string paragraf)
        {
            ViewBag.Paragraf = paragraf;
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(string fio, string email, string message)
        {
            return RedirectToAction("Index");
        }

        public ActionResult TopRetail()
        {
            var model = _home.GetTopProducts;
            return PartialView(model);
        }
    }
}