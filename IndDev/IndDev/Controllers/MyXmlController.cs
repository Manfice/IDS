using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;

namespace IndDev.Controllers
{
    [AllowAnonymous]
    public class MyXmlController : Controller
    {
        private readonly IXml _xml;

        public MyXmlController(IXml xml)
        {
            _xml = xml;
        }

        [Route("Site.xml")]
        public ActionResult Sitemap()
        {
            return this.Content(_xml.CreateSitemapDocument().ToString(), "text/xml", Encoding.UTF8);
        }
    }
}