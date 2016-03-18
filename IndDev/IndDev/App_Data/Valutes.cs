using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using IndDev.Entitys;

namespace IndDev.App_Data
{
    public class Valutes
    {
        private IEnumerable<Currency> AllCurrencies { get; set; }
        
        public Valutes()
        {
            var doc = XDocument.Load(ValDateReq(DateTime.Now));
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
                        ReqDate = d,
                        Stitle = element.Value,
                        Title = element1.Value,
                        CurValue = cursValue(xElement2.Value)
                    }).ToList();
                return valCbrfs;
            }
            return null;
        }
        private string ValDateReq(DateTime date)
        {
            const string cbrUri = "http://www.cbr.ru/scripts/XML_daily.asp";
            const string datereq = "?date_req=";
            DateTime reqday;

            var strDate = date.Day;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    reqday = date.AddDays(-1); break;
                case DayOfWeek.Sunday:
                    reqday = date.AddDays(-2);
                    break;
                default:
                    reqday = date;
                    break;
            }
            var dateForValls = new StringBuilder();
            dateForValls.Append(reqday.Day + "." + reqday.Month + "." + reqday.Year);
            return cbrUri + datereq + date.ToShortDateString();
        }

        private decimal cursValue(string curs)
        {
            var ds = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            if (ds == ".")
            {
                return decimal.Parse(curs.Replace(',', '.'));
            }
            else
            {
                return decimal.Parse(curs);
            }
        }
    }
}
