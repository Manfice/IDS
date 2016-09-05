using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class DbSearch : ISearchRepository
    {
        private readonly DataContext _context = new DataContext();
        public List<Product> GetProducts => _context.Products.ToList();

        public async Task<SearchModel> SearchResult(string request)
        {
            var products = await _context.Products.Select(product=> new SearchItem
            {
                Id = product.Id,
                Title = product.Title,
                Articul = product.Articul,
                Brand = product.Brand.FullName,
                Avatar = product.ProductPhotos.FirstOrDefault(p=>p.PhotoType==PhotoType.Avatar).Path,
                Rank = 0
            }).ToListAsync();

            var words = request.Split(new char[] {' ','.','/','\\','|',',','-'});

            var result = new SearchModel() {SearchRequest = request, SearchItems = new List<SearchItem>()};

            foreach (var product in products)
            {
                var tempId = 0;
                foreach (var tempItem in from word in words.Where(s => s!="").Distinct() where (product.Title.ToLower().Contains(word.ToLower())) || (product.Articul.ToLower().Contains(word.ToLower())) select result.SearchItems.FirstOrDefault(i => i.Id == product.Id))
                {
                    if (tempItem==null)
                    {
                        result.SearchItems.Add(product);
                        tempId = product.Id;
                    }
                    else
                    {
                        tempItem.Rank++;
                    }
                }
                var serchItem = tempId>0 ? result.SearchItems.FirstOrDefault(i => i.Id == tempId):null;
                if (serchItem == null) continue;
                foreach (var word in words.Where(s => s != "").Distinct())
                {
                    serchItem.Articul = Regex.Replace(serchItem.Articul, word, "<span>" + word + "</span>",
                        RegexOptions.IgnoreCase);
                    serchItem.Title = Regex.Replace(serchItem.Title, word, "<span>" + word + "</span>",
                        RegexOptions.IgnoreCase);
                    //serchItem.Articul = serchItem.Articul.ToLower().Replace(word, "<span>" + word + "</span>");
                    //serchItem.Title = serchItem.Title.ToLower().Replace(word, "<span>" + word + "</span>");
                }
            }
            var rank = result.SearchItems.Max(i => i.Rank);

            result.SearchItems = result.SearchItems.OrderByDescending(i => i.Rank).ToList();
            result.Total = result.SearchItems.Count;
            if (rank > 0)
            {
                result.SearchItems = result.SearchItems.Where(r => r.Rank == rank).ToList();
                result.Total = result.SearchItems.Count;
            }
            var sr = new SearchRequests()
            {
                SearchReq = request,
                SearchResult = result.Total.ToString()
            };
            _context.SearchRequestses.Add(sr);
            await _context.SaveChangesAsync();
            return result ;
        }
    }
}