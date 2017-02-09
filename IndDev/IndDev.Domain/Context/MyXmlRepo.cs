using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;

namespace IndDev.Domain.Context
{
    public class MyXmlRepo:IXml
    {
        private readonly DataContext _context = new DataContext();
        const string Dmn = "http://www.id-racks.ru/";
#region Sitemap.xml
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
            root = FillByProducts(root, ns);
            return root;
        }

        private XElement FillByProducts(XElement root, XNamespace ns)
        {
            var goods = _context.Products.Where(product => product.GetOptPrice().GetPriceRubl()>0).Select(product => new {product.CanonicTitle});
            foreach (var i in goods)
            {
                var node = new XElement(ns + "url",
                    new XElement(ns + "loc", $"{Dmn}Product/{i.CanonicTitle}"),
                    new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-mm-dd HH:MM")),
                    new XElement(ns + "changefreq", UpdateTime.Monthly),
                    new XElement(ns + "priority", "1.0"));
                root.Add(node);
            }

            return root;
        }
        #endregion
        #region ShopYaml.xml

        public XDocument CreateShopYml()
        {
            var xDoc = new XDocument(new XDocumentType("yml_catalog",null, "shops.dtd", null));
            var root = new XElement("yml_catalog", new XAttribute("date",DateTime.Now.ToString("yyyy-mm-dd HH:MM")));
            var shop = FillShopBlock();

            shop = FillShopByCat(shop);
            shop = FillOffers(shop);
            root.Add(shop);
            xDoc.Add(root);
            return xDoc;
        }

        private static XElement FillShopBlock()
        {
            var shop = new XElement("shop", 
                new XElement("name","АЙДИ-С"), 
                new XElement("company", @"Торговый дом АЙДИ-С"),
                new XElement("url", "http://www.id-racks.ru"),
                new XElement("currencies", new XElement("currency", new XAttribute("id","RUR"), new XAttribute("rate", "1"), new XAttribute("plus", "0"))),
                new XElement("categories"),
                new XElement("delivery-options", new XElement("option", new XAttribute("cost","500"), new XAttribute("days",""))),
                new XElement("offers"));
            
            return shop;
        }

        private XElement FillShopByCat(XElement shop)
        {
            var categorys = shop.Element("categories");
            if (categorys == null) return shop;

            var cat = _context.ProductMenuItems.Where(item => item.ShowInCatalog).OrderBy(item => item.ParentMenuItem.Id).ToList();
            foreach (var element in cat)
            {
                var c = new XElement("category", new XAttribute("id",element.Id),element.Title);
                if(element.ParentMenuItem!=null) c.Add(new XAttribute("parentId",element.ParentMenuItem.Id));
                categorys.Add(c);
            }

            return shop;
        }

        private XElement FillOffers(XElement shop)
        {
            var offers = shop.Element("offers");
            if (offers == null) return shop;

            var off = _context.Products.ToList();
            foreach (var product in off.Where(product => product.GetOptPrice().GetPriceRubl()>0))
            {
                var offer = new XElement("offer", new XAttribute("id",product.Id), new XAttribute("type", "vendor.model"), new XAttribute("available","true"));
                offer.Add(new XElement("url",$"http://www.id-racks.ru/product/{product.CanonicTitle}"));
                offer.Add(new XElement("price", product.GetOptPrice().GetPriceRubl()));
                offer.Add(new XElement("currencyId","RUR"));
                offer.Add(new XElement("categoryId", product.Categoy.Id));
                if (product.ProductPhotos.Any(photo => photo.PhotoType==PhotoType.Avatar))
                {
                    offer.Add(new XElement("picture", $"http://www.id-racks.ru{product.ProductPhotos.FirstOrDefault(photo => photo.PhotoType == PhotoType.Avatar)?.Path}"));
                }
                offer.Add(new XElement("delivery", "true"));
                offer.Add(new XElement("typePrefix", product.Categoy.Title));
                offer.Add(new XElement("vendor", product.Brand.FullName));
                offer.Add(new XElement("model", product.Articul));
                if (!string.IsNullOrEmpty(product.ShotDescription))
                {
                    offer.Add(new XElement("description", product.ShotDescription));
                }
                offer.Add(new XElement("manufacturer_warranty", "true"));
                offers.Add(offer);

            }
            return shop;
        }
#endregion
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