using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ExternalRes.Enity;

namespace ExternalRes
{
    public class Valutes
    {
        private IEnumerable<Currency> AllCurrencies { get; set; }

        public Valutes(XDocument doc)
        {
            //var doc = XDocument.Load("http://www.cbr.ru/scripts/XML_daily.asp");
            AllCurrencies=RequestCurrencies(doc);
        }

        public Currency GetUsd()
        {
            var usd = new Currency();
            foreach (var item in AllCurrencies.Where(item => item.Stitle=="USD"))
            {
                usd = item;
            }
            return usd;
        }
        public Currency GetEur()
        {
            var usd = new Currency();
            foreach (var item in AllCurrencies.Where(item => item.Stitle=="EUR"))
            {
                usd = item;
            }
            return usd;
        }

        private IEnumerable<Currency> RequestCurrencies(XDocument document)
        {
            var doc = document.Element("ValCurs");

            if (doc != null)
            {
                var d = doc.Attribute("Date").Value;
                var valCbrfs = (from c in doc.Descendants("Valute")
                    let xElement = c.Element("NumCode")
                    where xElement != null
                    let element = c.Element("CharCode")
                    where element != null
                    let xElement1 = c.Element("Nominal")
                    where xElement1 != null
                    let element1 = c.Element("Name")
                    where element1 != null
                    let xElement2 = c.Element("Value")
                    where xElement2 != null
                    select new Currency()
                    {
                        ReqDate = DateTime.Parse(d),
                        Stitle = element.Value,
                        Title = element1.Value,
                        CurValue = decimal.Parse(xElement2.Value)
                    }).ToList();
                return valCbrfs;
            }
            return null;
        }
    }
}
