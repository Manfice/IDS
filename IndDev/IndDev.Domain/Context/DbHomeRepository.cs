using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class DbHomeRepository : IHomeRepository
    {
        private readonly DataContext _context = new DataContext();

        public IEnumerable<ProductView> GetTopProducts
        {
            get
            {
                var products =
                    _context.Products.Where(
                        p => p.Prices.FirstOrDefault(price => price.PriceType == PriceType.Sale).Value > 0).ToList();
                var model = products.Select(product => new ProductView
                {
                    Product = product,
                    Avatar = product.ProductPhotos.FirstOrDefault(photo => photo.PhotoType==PhotoType.Avatar),
                    Prices = product.Prices.Where(price => price.PriceType==PriceType.Sale).Select(price => new PriceViewModel
                    {
                        Product = product,
                        Title = price.Title,
                        PriceType = price.PriceType,
                        Id = price.Id,
                        Currency = price.Currency,
                        OriginalPrice = price.Value,
                        PriceFrom = price.QuanttityFrom
                    }).ToList()
                }).ToList();

                return model;
            }
        }

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