using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class ExceptionPageController : Controller
    {
        // GET: ExceptionPage
        public ActionResult Page404()
        {
            return View();
        }
    }
}