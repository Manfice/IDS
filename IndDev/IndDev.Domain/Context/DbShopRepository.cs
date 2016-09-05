﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;
using System.Threading.Tasks;

namespace IndDev.Domain.Context
{
    public class DbShopRepository:IShopRepository
    {
         private readonly DataContext _context = new DataContext();

        public IEnumerable<ProductMenu> GetProductMenus => _context.ProductMenus.Where(menu => menu.ShowInCatalog).OrderBy(menu => menu.Priority).ToList();
        public IEnumerable<Product> GetProducts => _context.Products.ToList();

        public ProductMenu GetProductMenu(int id)
        {
            return _context.ProductMenus.Find(id);
        }

        private bool IsSale(List<PriceViewModel> model)
        {
            return model.Any(item => item.Title == "Sale" && item.ConvValue > 0);
        }

        private bool CheckPrice(List<PriceViewModel> list)
        {
            return list != null && list.Any(item => item.ConvValue>0);
        }

        public IEnumerable<Menu> GetSubMenu(int id)
        {
            return _context.Menus.Where(menu => menu.ParentItem.Id==id).ToList();
        }

        public IEnumerable<Menu> GetTopMenus()
        {
            return _context.Menus.Where(menu => menu.ParentItem==null).ToList();
        }

        public ShopProductView GetProduct(int id) //menu item ID
        {
            var menuItem = _context.ProductMenuItems.Find(id);
            var products = menuItem.Products.Select(product => new ProductView
            {
                Product = product, Avatar = product.ProductPhotos.FirstOrDefault(p => p.PhotoType == PhotoType.Avatar), Prices = product.Prices.Where(price => price.Value > 0).Select(price => new PriceViewModel {Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom, PriceType = price.PriceType}).ToList()
            }).ToList();
            var sCat = _context.ProductMenuItems.Where(item => item.ParentMenuItem.Id == id);
            return new ShopProductView {ProductMenuItem = menuItem, Products = products, MenuItems = sCat};
        }

        public async Task<ShopProductView> SearchProductsAsynk(string request)
        {
            var result = await SearchResultAsync(request);
            var pV = new List<ProductView>();
            foreach (var item in result.SearchItems)
            {
                var pr = await _context.Products.FindAsync(item.Id);
                if (pr==null) continue;
                var prv = new Product();
                prv = pr;
                prv.Title = item.Title;
                prv.Rate = item.Rank;
                pV.Add(new ProductView
                {
                    Product = prv,
                    Avatar = pr.ProductPhotos.FirstOrDefault(p => p.PhotoType == PhotoType.Avatar),
                    Prices = pr.Prices.Where(price => price.Value > 0).Select(price => new PriceViewModel { Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom, PriceType = price.PriceType }).ToList()
                });
            }
            return new ShopProductView() {Products = pV};
        }

        public ProductView GetProductDetails(int id)
        {
            var prod = _context.Products.Find(id);
            var pvm = new ProductView
            {
                Product = prod,
                Avatar = prod.ProductPhotos.FirstOrDefault(p => p.PhotoType == PhotoType.Avatar),
                Prices = prod.Prices.Where(price => price.Value > 0).Select(price => new PriceViewModel { Id = price.Id, Currency = price.Currency, OriginalPrice = price.Value, Title = price.Title, PriceFrom = price.QuanttityFrom, PriceType = price.PriceType}).ToList()
            };
            return pvm;
        }

        public void SaveSearch(SearchRequests model)
        {
            _context.SearchRequestses.Add(model);
            _context.SaveChanges();
        }

        private async Task<SearchModel> SearchResultAsync(string request)
        {
            var products = await _context.Products.Select(product => new SearchItem
            {
                Id = product.Id,
                Title = product.Title,
                Articul = product.Articul,
                Brand = product.Brand.FullName,
                Avatar = product.ProductPhotos.FirstOrDefault(p => p.PhotoType == PhotoType.Avatar).Path,
                Rank = 0
            }).ToListAsync();

            var words = request.Split(new char[] { ' ', '.', '/', '\\', '|', ',', '-' });

            var result = new SearchModel() { SearchRequest = request, SearchItems = new List<SearchItem>() };

            foreach (var product in products)
            {
                var tempId = 0;
                foreach (var tempItem in from word in words.Where(s => s != "").Distinct() where (product.Title.ToLower().Contains(word.ToLower())) || (product.Articul.ToLower().Contains(word.ToLower())) select result.SearchItems.FirstOrDefault(i => i.Id == product.Id))
                {
                    if (tempItem == null)
                    {
                        result.SearchItems.Add(product);
                        tempId = product.Id;
                    }
                    else
                    {
                        tempItem.Rank++;
                    }
                }
                var serchItem = tempId > 0 ? result.SearchItems.FirstOrDefault(i => i.Id == tempId) : null;
                if (serchItem == null) continue;
                foreach (var word in words.Where(s => s != "").Distinct())
                {
                    serchItem.Articul = Regex.Replace(serchItem.Articul, word, "<span>" + word + "</span>",
                        RegexOptions.IgnoreCase);
                    serchItem.Title = Regex.Replace(serchItem.Title, word, "<span>" + word + "</span>",
                        RegexOptions.IgnoreCase);
                }
            }
            var rank = result.SearchItems.Max(i => i.Rank);

            result.SearchItems = result.SearchItems.OrderByDescending(i => i.Rank).ToList();
            result.Total = result.SearchItems.Count;
            if (rank <= 0) return result;
            result.SearchItems = result.SearchItems.Where(r => r.Rank == rank).ToList();
            result.Total = result.SearchItems.Count;
            return result;
        }
    }
}