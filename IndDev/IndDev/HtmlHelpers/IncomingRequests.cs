using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace IndDev.HtmlHelpers
{
    public static class IncomingRequests
    {
        private static readonly Dictionary<int, string> Visitors; 
        static IncomingRequests()
        {
            Visitors = new Dictionary<int, string>();
        }

        public static void RegisterNewEntry(string url)
        {
            if (Visitors.Count>99)
            {
                FreeVisitors();
            }
            var c = Visitors.Count;
            Visitors.Add(c, url);
        }

        private static void FreeVisitors()
        {
            var sb = new StringBuilder();
            foreach (var data in Visitors)
            {
                sb.Append(data.Key).Append(" : ").Append(data.Value).Append("\r\n");
            }
            var result = sb.ToString();
            var fp = HttpContext.Current.Server.MapPath($"~/Content/zzz{Guid.NewGuid()}.txt");
            File.WriteAllText(fp, result);
            Visitors.Clear();
        }
    }
}