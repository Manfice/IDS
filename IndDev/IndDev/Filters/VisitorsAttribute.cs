using System;
using System.Web.Mvc;
using IndDev.HtmlHelpers;

namespace IndDev.Filters
{
    public class VisitorsAttribute : FilterAttribute, IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            IncomingRequests.RegisterNewEntry(filterContext.HttpContext.Request.RawUrl);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}