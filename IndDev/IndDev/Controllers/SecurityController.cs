using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IndDev.Domain.Abstract;
using IndDev.Auth.Model;
using Microsoft.Ajax.Utilities;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        private readonly ISecureRepository _repository;
        private readonly IMailRepository _mailRepository;

        public SecurityController(ISecureRepository repository, IMailRepository mail)
        {
            _repository = repository;
            _mailRepository = mail;
        }
        // GET: Security
        public ActionResult Login(string returnUrl) 
        {
            var login = new LoginViewModel() {ReturnUrl = returnUrl};
            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) 
        {
            if (ModelState.IsValid)
                {
                    var svm = _repository.Login(model);
                    if (svm.Code>0)
                    {
                        FormsAuthentication.SetAuthCookie(svm.Code.ToString(),model.RememberMe);
                        TempData["secureMessage"] = string.Format(svm.Message);
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            string decodeUrl = Server.UrlDecode(returnUrl);
                            if (Url.IsLocalUrl(decodeUrl))
                            {
                                return Redirect(decodeUrl);
                            }
                        }
                    }
                    TempData["secureMessage"] = string.Format(svm.Message);
                    return RedirectToAction("Landing","Home");
                }
            
            TempData["secureMessage"] = string.Format("Указаны не корректные данные.");
            return View(model);
        }
        public ActionResult Register (string returnUrl) 
        {
            var login = new RegisterViewModel() {ReturnUrl = returnUrl};
            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register (RegisterViewModel register, string  returnUrl) 
        {
            if (ModelState.IsValid)
            {
                var rvm = _repository.Register(register);
                if (rvm.Code==0)
                {
                    FormsAuthentication.SetAuthCookie(_repository.GetUser(register.Email).Id.ToString(),false);
                    TempData["secureMessage"] = string.Format(rvm.Message);
                    var messageBody = System.IO.File.ReadAllText(Server.MapPath("~/Views/Mails/ThankYou.html"));
                    await _mailRepository.RegisterLetterAsync(messageBody,register);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        string decodeUrl = Server.UrlDecode(returnUrl);
                        if (Url.IsLocalUrl(decodeUrl))
                        {
                            return Redirect(decodeUrl);
                        }
                        return RedirectToAction("Landing", "Home");
                    }
                    TempData["secureMessage"] = string.Format(rvm.Message);
                    return RedirectToAction("Landing", "Home");
                }
            TempData["secureMessage"] = rvm.Message;
            }
            return View(register);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Landing","Home");
        }
    }
}