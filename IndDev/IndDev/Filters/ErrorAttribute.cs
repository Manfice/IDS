using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IndDev.Filters
{
    public class ErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.ExceptionHandled = true;
            }
        }
    }
}