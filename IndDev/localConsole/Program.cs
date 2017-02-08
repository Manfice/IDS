using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IndDev.Domain.Context;

namespace localConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckSiteXml();
            Console.ReadLine();
        }

        private static void CheckSiteXml()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var xDoc = XDocument.Load("http://www.id-racks.ru/site.xml");

            var urls = xDoc.Element(ns + "urlset").Elements(ns + "url");
            var ind = 0;
            var errList = new List<string>();
            foreach (var x in urls)
            {
                ind++;
                var url = x.Element(ns + "loc").Value;
                try
                {
                    var wrq = (HttpWebRequest)WebRequest.Create(url);
                    var wrp = (HttpWebResponse)wrq.GetResponse();
                    Console.WriteLine($"{ind}:{url}");
                    Console.WriteLine($"{wrp.Method}: {wrp.StatusDescription}");
                    wrp.Close();
                }
                catch (Exception e)
                {
                    var s = $"{e.Message}:{ind}:{url}";
                    errList.Add(s);
                }
                Console.WriteLine("-------------------------------");

            }
            if (errList.Any())
            {
                ind = 0;
                foreach (var s in errList)
                {
                    ind++;
                    Console.WriteLine($"{ind}:{s}");
                }

            }
            Console.WriteLine("END");
        }
    }
}
