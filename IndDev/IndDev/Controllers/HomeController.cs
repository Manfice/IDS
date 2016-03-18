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

        public HomeController(ISecureRepository repository)
        {
            _repository = repository;
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
            var cur = new Valutes(DateTime.Today).GetCurses();
            return PartialView(cur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(string fio, string email, string message)
        {
            return RedirectToAction("Landing");
        }
    }
}