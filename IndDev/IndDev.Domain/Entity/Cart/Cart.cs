using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Entity.Cart
{
    public class Cart
    {
        private readonly List<CartItem> _cartItems = new List<CartItem>();
        public IEnumerable<CartItem> CartItems => _cartItems;
        public Customer Customer { get; set; }


        public void AddItem(Product product, int quantity)
        {
            var cItem = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (cItem == null)
            {
                _cartItems.Add(new CartItem(product)
                {
                    Quantity = quantity
                });
            }
            else
            {
                cItem.Quantity += quantity;
            }
        }

        public void ChangeQuantity(Product product, int quantity)
        {
            var line = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (line == null) return;
            line.Quantity = quantity;
        }

        public void RemoveLine(Product product)
        {
            _cartItems.RemoveAll(item => item.Product.Id == product.Id);
        }

        public decimal CalcTotalSumm()
        {
            var summ = _cartItems.Sum(item => Math.Round(item.Quantity * item.RetailPrice.ConvValue, 2));
            return summ;
        }

        public decimal CalcTotalWithDiscount()
        {
            var summ = _cartItems.Sum(item => Math.Round(item.Quantity * item.RetailPrice.ConvValue, 2));
            if (Customer == null) return summ;
            if (Customer.CustomerStatus.Discount > 0)
            {
                summ = summ - ((summ * Customer.CustomerStatus.Discount) / 100);
            }
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
        public PriceViewModel RetailPrice { get; }

        public int Quantity { get; set; }
        public decimal SubTotal => Math.Round(Quantity*RetailPrice.ConvValue,2);

        public CartItem(Product product)
        {
            Product = product;
            RetailPrice = new PriceViewModel();
            var priceRetail = product.Prices.FirstOrDefault(p => p.PriceType == PriceType.Retail);
            var priceSale = product.Prices.FirstOrDefault(p=>p.PriceType==PriceType.Sale);

            if (priceSale != null && priceSale.Value > 0) priceRetail = priceSale;
            if (priceRetail == null || priceRetail.Value <= 0) return;
            RetailPrice.Id = priceRetail.Id;
            RetailPrice.Currency = priceRetail.Currency;
            RetailPrice.OriginalPrice = priceRetail.Value;
        }
    }
}