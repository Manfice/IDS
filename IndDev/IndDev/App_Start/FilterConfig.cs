using System.Web;
using System.Web.Mvc;
using IndDev.Filters;

namespace IndDev
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            //filters.Add(new VisitorsAttribute());
            
        }
    }
}
