using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IndDev.Domain.Context
{
    public class Valutes
    {
        private readonly DataContext _context = new DataContext();
        private IEnumerable<Curency> AllCurrencies { get; set; }
        
        public Valutes(DateTime date)
        {
            AllCurrencies = GetCursOnDate(date);
        }

        public IEnumerable<Curency> GetCurses()
        {
            return AllCurrencies.ToList();
        } 

        public Curency GetUsd()
        {
            var usd = new Curency();
            foreach (var item in AllCurrencies.Where(item => item.Stitle=="USD"))
            {
                usd = item;
            }
            return usd;
        }
        public Curency GetEur()
        {
            var usd = new Curency();
            foreach (var item in AllCurrencies.Where(item => item.Stitle=="EUR"))
            {
                usd = item;
            }
            return usd;
        }

        private IEnumerable<Curency> GetCursOnDate(DateTime date)
        {

            var cDate = CorrWeekDay(date);
            var curses = _context.Curses.Where(curency => curency.ReqDate==cDate).ToList();
            if (curses.Any())
            {
                return curses;
            }
            try
            {
                var req = ValDateReq(date);
                var doc = XDocument.Load(req);
                curses = RequestCurrencies(doc).ToList();
            }
            catch (Exception e)
            {
                return new List<Curency>
                {
                    new Curency {Stitle = e.Message,CurValue = 0, Title = date.ToString(CultureInfo.CurrentCulture)}
                };
            }
            foreach (var curency in curses.Where(curency => (curency.Stitle=="USD") || (curency.Stitle=="EUR")))
            {
                var curs = new Curency
                {
                    CurValue = curency.CurValue,
                    ReqDate = curency.ReqDate,
                    Stitle = curency.Stitle,
                    Title = curency.Title
                };
                _context.Curses.Add(curs);
                var currentCursVall = _context.Currencies.FirstOrDefault(currency => currency.Code == curency.Stitle);
                currentCursVall.ActualDate=DateTime.Today;
                currentCursVall.Curs = curency.CurValue;
            }
            _context.SaveChanges();
            curses = _context.Curses.Where(curency => curency.ReqDate == cDate).ToList();
            return curses;
        } 

        private IEnumerable<Curency> RequestCurrencies(XDocument document)
        {
            var doc = document.Element("ValCurs");
            if (doc != null)
            {
                var d = doc.Attribute("Date").Value;
                DateTime docDate = makeDate(d);
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
                    select new Curency()
                    {
                        ReqDate = docDate,
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
            const string cbrUri = "http://www.cbr.ru/scripts/XML_daily.asp";  //http://www.cbr.ru/scripts/XML_daily.asp?date_req=15.03.2016
            const string datereq = "?date_req=";
            var result = cbrUri + datereq + CorrDate(date);
            return result;
        }

        private DateTime CorrWeekDay(DateTime date)
        {
            var dd = date;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dd = date.AddDays(-1);
                    break;
                case DayOfWeek.Monday:
                    dd = date.AddDays(-2);
                    break;
            }
            return dd;
        }
        private string CorrDate(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    date = date.AddDays(-1);
                    break;
                case DayOfWeek.Monday:
                    date = date.AddDays(-2);
                    break;
            }

            var day = date.Day.ToString("00");
            var month = date.Month.ToString("00");
            var year = date.Year.ToString("0000");

            var result = new StringBuilder();
            result.Append(day);
            result.Append('.');
            result.Append(month);
            result.Append('.');
            result.Append(year);
            return result.ToString();
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

        private DateTime makeDate(string s)
        {
            var d = s.Split('.');
            var day = int.Parse(d[0]);
            var mnth = int.Parse(d[1]);
            var year = int.Parse(d[2]);
            var result = new DateTime(year,mnth,day);
            return result;
        }
    }
}
