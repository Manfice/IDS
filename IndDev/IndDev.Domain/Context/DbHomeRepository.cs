using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class DbHomeRepository : IHomeRepository
    {
        private readonly DataContext _context = new DataContext();

        public CursViewModel GetCurses(DateTime date)
        {
            var needToUpdate = false;
            var cr = new List<Curency>();
            var actualCurses = _context.Currencies.Where(currency => currency.Code != "RUB").ToList();
            foreach (var curse in actualCurses.Where(curse => curse.ActualDate!=date))
            {
                needToUpdate = true;
            }
            if (needToUpdate)
            {
                var crToUpdate = new Valutes(date).GetCurses().ToList();
                foreach (var curency in crToUpdate)
                {
                    var cur = _context.Currencies.FirstOrDefault(currency => currency.Code == curency.Stitle);
                    cur.ActualDate = DateTime.Today;
                    cur.Curs = curency.CurValue;
                }
                _context.SaveChanges();
            }
            cr.AddRange(actualCurses.Select(curse => new Curency
            {
                CurValue = curse.Curs, Stitle = curse.Code, Title = curse.StringCode, ReqDate = curse.ActualDate
            }));
            return new CursViewModel
            {
                Curses = cr, CursDate = date
            };
        }
    }
}