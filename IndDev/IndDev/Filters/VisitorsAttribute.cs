using System;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Context;
using IndDev.HtmlHelpers;
using Newtonsoft.Json;

namespace IndDev.Filters
{
    public class VisitorsAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //IncomingRequests.RegisterNewEntry(filterContext.HttpContext.Request.RawUrl);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext.Request;
            var ctr = filterContext.HttpContext.Response;
            if (!ctx.Browser.Cookies)
            {
                return;
            }
            var id = Guid.NewGuid().ToString();
            var coockieReq = ctx.Cookies["id_vtc"];
            if (coockieReq != null)
            {
                id = coockieReq["id_vtc_id"];
            }
            else
            {
                coockieReq = new HttpCookie("id_vtc") {["id_vtc_id"] = id, Expires = DateTime.Today.AddDays(120)};
                ctr.Cookies.Add(coockieReq);
            }
        }
    }
}