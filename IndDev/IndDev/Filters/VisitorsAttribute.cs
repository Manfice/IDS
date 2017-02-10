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

        }
    }
}