using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using IndDev.HtmlHelpers;

namespace IndDev.Infrastructure
{
    public class VisitorModule : IHttpModule
    {
        private static int sharedCounter = 0;
        private int reqCounter;

        private static object lockObj = new object();
        private Exception reqException = null;


        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            //context.BeginRequest += (sender, args) => { reqCounter = ++sharedCounter; };
            //context.Error += (sender, args) => { reqException = HttpContext.Current.Error; };
            //context.LogRequest += (sender, args) => { WriteMessage(HttpContext.Current);};
        }

        private void WriteMessage(HttpContext ctx)
        {
            var sb = new StringWriter();
            sb.WriteLine("-------------------------------------");
            sb.WriteLine($"Request: {reqCounter} for: {ctx.Request.Url}");
            if (ctx.Handler!=null)
            {
                sb.WriteLine($"Handler: {ctx.Handler.GetType()}");
            }
            sb.WriteLine($"Status code: {ctx.Response.StatusCode} Message: {ctx.Response.StatusDescription}");
            sb.WriteLine($"Elepsed time: {DateTime.Now.Subtract(ctx.Timestamp).Milliseconds} ms");
            if (reqException!=null)
            {
                sb.WriteLine($"Error: {reqException.GetType()}");
            }
            IncomingRequests.RegisterNewEntry(sb.ToString());

        }

        private void GetVisitor(object src, EventArgs e)
        {
            var ctx = HttpContext.Current;
            if (ctx.CurrentNotification == RequestNotification.BeginRequest)
            {
                IncomingRequests.RegisterNewEntry(ctx.Request.RawUrl);
            }
        }
    }
}