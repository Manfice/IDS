using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Logic
{
    public static class Sitemap
    {
        private const string xmlStr = @"
            <url>
                <loc>{0}</loc>                
                <changefreq>monthly</changefreq>
                <priority>1</priority>
            </url>
        ";
        private const string prodUrl = @"http://www.id-racks.ru/Shop/ProductDetails?id=";
        private const string mnsm = @"<?xml version=""1.0"" encoding=""utf-8"" ?><urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9""> ";

        public static string MakeXmlList(IEnumerable<Product> products)
        {
            if (!products.Any())
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.Append(mnsm+Environment.NewLine);
            foreach (var product in products)
            {
                sb.Append(FormatProduct(product)+Environment.NewLine);
            }
            sb.Append("</urlset>");
            return sb.ToString();
        }

        private static string FormatProduct(Product product)
        {
            return string.Format(xmlStr, prodUrl+product.Id);
        }


    }
}