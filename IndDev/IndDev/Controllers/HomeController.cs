using System;
using System.Security.Cryptography;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Context;

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
           return View();
        }
        
        public ActionResult About()
        {
            return View();
        }
        
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Landing()
        {
            return View();
        }

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

        public ActionResult MessageScreen(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(string fio, string email, string message)
        {
            return RedirectToAction("Landing");
        }
    }
}