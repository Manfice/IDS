using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Models;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class MailController : Controller
    {
        private IMailRepository _repository;

        public MailController(IMailRepository repository)
        {
            _repository = repository;
        }
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendQuestion(MailMessageModel message)
        {
            if (!ModelState.IsValid) return View("Error");
            var bdy = System.IO.File.ReadAllText(Server.MapPath("~/Views/Mails/LetterFromTitle.html"));
            _repository.SendMessage(message,bdy);
            return RedirectToAction("About", "Home");
        }
    }
}