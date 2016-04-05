using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Entity.Cart
{
    public class Cart
    {
        private readonly List<CartItem> _cartItems = new List<CartItem>();
        public IEnumerable<CartItem> CartItems => _cartItems;

        private static PriceViewModel GetActualPrice(Product product, int quantity)
        {
            PriceViewModel p = null;
            foreach (var source in product.Prices.Where(price1 => price1.Value > 0).Where(source => quantity >= source.QuanttityFrom))
            {
                if (p==null)
                {
                    p = new PriceViewModel
                    {
                        Id = source.Id,
                        Product = source.Product,
                        Title = source.Title,
                        Currency = source.Currency,
                        OriginalPrice = source.Value,
                        PriceFrom = source.QuanttityFrom
                    };
                }
            }
            return p;
        }

        public void AddItem(Product product, int quantity)
        {
            var cItem = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (cItem == null)
            {
                _cartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    ActualPrice = GetActualPrice(product,quantity)
                });
            }
            else
            {
                cItem.Quantity += quantity;
                cItem.ActualPrice = GetActualPrice(product, cItem.Quantity);
            }
        }

        public void ChangeQuantity(Product product, int quantity)
        {
            var line = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (line == null) return;
            line.Quantity = quantity;
            line.ActualPrice = GetActualPrice(product, quantity);
        }

        public void RemoveLine(Product product)
        {
            _cartItems.RemoveAll(item => item.Product.Id == product.Id);
        }

        public decimal CalcTotalSumm()
        {
            var summ = _cartItems.Sum(item => Math.Round(item.Quantity * item.ActualPrice.ConvValue, 2));
            return summ;
        }

        public void ClearList()
        {
            _cartItems.Clear();
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public PriceViewModel ActualPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Math.Round(Quantity*ActualPrice.ConvValue,2);
    }
}