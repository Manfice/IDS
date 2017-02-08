using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IndDev.Domain.Abstract;

namespace IndDev.Domain.Context
{
    public class MyXmlRepo:IXml
    {
        private readonly DataContext _context = new DataContext();
        const string Dmn = "http://www.id-racks.ru/";

        public XDocument CreateSitemapDocument()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var xDoc = new XDocument();

            var root = new XElement(ns+"urlset");

            root = GetStaticElements(root, ns);
            xDoc.Add(root);
            return xDoc;
        }

        private XElement GetStaticElements(XElement root, XNamespace ns)
        {
            var sn = new List<StaticNode>
            {
                new StaticNode {Url = $"{Dmn}", Update = UpdateTime.Daily, Priority = "0.1"},
                new StaticNode {Url = $"{Dmn}Catalog", Update = UpdateTime.Weekly, Priority = "0.7"},
                new StaticNode {Url = $"{Dmn}Company", Update = UpdateTime.Yearly, Priority = "0.1"},
                new StaticNode {Url = $"{Dmn}Contacts", Update = UpdateTime.Yearly, Priority = "0.1"},
                new StaticNode {Url = $"{Dmn}Baraholochka", Update = UpdateTime.Daily, Priority = "1.0"}
            };

            foreach (var node in sn.Select(item => new XElement(ns+"url",
                new XElement(ns + "loc", item.Url),
                new XElement(ns + "changefreq", item.Update.ToString()),
                new XElement(ns + "priority", item.Priority))))
            {
                root.Add(node);
            }
            root = FillByGroups(root, ns);

            return root;
        }

        private XElement FillByGroups(XElement root, XNamespace ns)
        {

            var groups = _context.ProductMenus.Where(menu => menu.ShowInCatalog).Select(menu => new
            {
                menu.CanonicalTitle
            });

            foreach (var i in groups)
            {
                var node = new XElement(ns+"url",
                    new XElement(ns+"loc", $"{Dmn}catalog/{i.CanonicalTitle}"),
                    new XElement(ns+ "changefreq", UpdateTime.Monthly),
                    new XElement(ns+ "priority", "0.9"));
                root.Add(node);
            }
            root = FillBySubGroups(root, ns);
            return root;
        }

        private XElement FillBySubGroups(XElement root, XNamespace ns)
        {
            var groups = _context.ProductMenuItems.Where(menu => menu.ShowInCatalog).Select(menu => new
            {
                menu.CanonicalTitle
            });

            foreach (var i in groups)
            {
                var node = new XElement(ns + "url",
                    new XElement(ns + "loc", $"{Dmn}Category/{i.CanonicalTitle}"),
                    new XElement(ns + "changefreq", UpdateTime.Monthly),
                    new XElement(ns + "priority", "0.9"));
                root.Add(node);
            }
            return root;
        }

    }

    public enum UpdateTime
    {
        Always, Hourly, Daily, Weekly, Monthly, Yearly, Never
    }

    public class StaticNode
    {
        public string Url { get; set; }
        public UpdateTime Update { get; set; }
        public string Priority { get; set; }
    }
}