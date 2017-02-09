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
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) =>
            {
                if (HttpContext.Current.Request.Url.Host.StartsWith("www") || HttpContext.Current.Request.Url.IsLoopback)
                    return;
                var builder = new UriBuilder(HttpContext.Current.Request.Url)
                {
                    Host = $"www.{HttpContext.Current.Request.Url.Host}"
                };
                HttpContext.Current.Response.StatusCode = 301;
                HttpContext.Current.Response.AddHeader("Location", builder.ToString());
                HttpContext.Current.Response.End();
            };
        }
    }
}