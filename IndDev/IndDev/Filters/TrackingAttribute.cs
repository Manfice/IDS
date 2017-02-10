using System;
using System.Web;
using System.Web.Mvc;
using IndDev.Domain.Context;
using IndDev.Domain.Entity.TrackingUser;

namespace IndDev.Filters
{
    public class TrackingAttribute : FilterAttribute, IActionFilter
    {
        Tracking t = new Tracking();
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            var visitor = new Visitor(ctx);
            var userRout = new UserRout(ctx)
            {
                ActionVisited = filterContext.ActionDescriptor.ActionName,
                ControllerVisited = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
            };

            if (!ctx.Request.Browser.Cookies)
            {
                return;
            }
            var id = Guid.NewGuid().ToString();
            var coockieReq = ctx.Request.Cookies["id_vtc"];
            if (coockieReq != null)
            {
                visitor.Identifer = coockieReq["id_vtc_id"];
                visitor.Email = coockieReq["id_vtc_mail"];
            }
            else
            {
                coockieReq = new HttpCookie("id_vtc") { ["id_vtc_id"] = id, ["id_vtc_mail"]="", Expires = DateTime.Today.AddDays(120) };
                visitor.Identifer = id;
                ctx.Response.Cookies.Add(coockieReq);
            }
            t.CreateNewVisitor(visitor, userRout);
        }
    }
}