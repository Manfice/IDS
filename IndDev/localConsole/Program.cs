using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
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
            Console.WriteLine("Start ---------->");
            var ind = 0;
            using (var db = new DataContext())
            {
                var p = db.Visitors.ToList();
                //foreach (var rout in p.UserRouts)
                //{
                //    Console.WriteLine($"{rout.ActionVisited} - {rout.ControllerVisited} - {rout.UrlString} - {rout.UrlQuery}");
                //}
                var ua = p.Select(visitor => visitor.UserAgent).Distinct();
                //foreach (var s in ua)
                //{
                //    Console.WriteLine(s);
                //}
                foreach (var visitor in p)
                {
                    ind++;
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"{ind}:{visitor.Identifer}");
                    Console.WriteLine($"{visitor.FirstDate} : {visitor.UserAgent}");
                    Console.WriteLine($"{visitor.StartUrl} : {visitor.UserRouts.Count}");
                }


            }
            //CheckSiteXml();
            Console.WriteLine("END --------->");
            Console.ReadLine();
        }

        private static void CheckSiteXml()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var xDoc = XDocument.Load("http://www.id-racks.ru/sitemap.xml");

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
//var pt = product.Title.Split(new[] { ' ', '.', ',', '*', '"', '/', '\\', '\t', ':', '(', ')' });
//var art = product.Articul.Replace(".", ",");
//ind++;
//Console.WriteLine($"-----{ind}-----");
//Console.WriteLine($"{art}-{string.Join("_", pt.Where(s => s.Length > 1).Select(s => s.Replace("+", "_Plus")).Take(3))}");
//Console.WriteLine();
//var dbProd = db.Products.Find(product.Id);
//if (dbProd != null)
//{
//    dbProd.CanonicTitle = $"{art}-{string.Join("_", pt.Where(s => s.Length > 1).Select(s => s.Replace("+", "_Plus")).Take(3))}";
//}
//if ((ind % 100) == 0)
//{
//db.SaveChanges();
//}