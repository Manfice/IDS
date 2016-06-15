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
        private Customer _customer;
        public IEnumerable<CartItem> CartItems
        {
            get
            {
                UpdatePrice();
                return _cartItems;
            }
        }

        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                UpdatePrice();
            }
        }

        private void SetPrice(CartItem item, PriceType priceType)
        {
            var price = item.Product.Prices.FirstOrDefault(p => p.PriceType == priceType);
            if (price != null && price.Value > 0)
            {
                item.ActualPrice = new PriceViewModel
                {
                    Id = price.Id,
                    Title = price.Title,
                    Product = price.Product,
                    Currency = price.Currency,
                    OriginalPrice = price.Value
                };
            }
            else
            {
                item.ActualPrice = item.RetailPrice;
            }
        }

        private void UpdatePrice()
        {
            var curentTotal = CalcTotalSumm();
            if (curentTotal>=350000 && Customer==null)
            {
                foreach (var item in _cartItems)
                {
                    SetPrice(item,PriceType.Opt);
                }
            }else if (curentTotal>=60000 && Customer==null)
            {
                foreach (var item in _cartItems)
                {
                    SetPrice(item,PriceType.LowOpt);
                }
            }else if (curentTotal<60000 && Customer==null)
            {
                foreach (var item in _cartItems)
                {
                    SetPrice(item,item.RetailPrice.PriceType);
                }
            }
            if (Customer != null && curentTotal >= 350000)
            {
                foreach (var item in _cartItems)
                {
                    SetPrice(item, PriceType.Opt);
                }
            }
            else
            {
                foreach (var item in _cartItems)
                {
                    if (Customer != null) SetPrice(item,Customer.CustomerStatus.PriceType);
                }
            }
        }

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
            UpdatePrice();
        }

        public void ChangeQuantity(Product product, int quantity)
        {
            var line = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (line == null) return;
            line.Quantity = quantity;
            UpdatePrice();
        }

        public void RemoveLine(Product product)
        {
            _cartItems.RemoveAll(item => item.Product.Id == product.Id);
            UpdatePrice();
        }


        public decimal CalcTotalSumm()
        {
            var summ = _cartItems.Sum(item => Math.Round(item.Quantity * item.RetailPrice.ConvValue, 2));
            return summ;
        }
        public decimal CalcActualTotalWithDiscount()
        {
            var summ = _cartItems.Sum(item => Math.Round(item.Quantity * item.ActualPrice.ConvValue, 2));
            if (Customer == null) return summ;
            if (Customer.CustomerStatus.Discount > 0)
            {
                summ = summ - ((summ * Customer.CustomerStatus.Discount) / 100);
            }
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
        public decimal SubTotal => Math.Round(Quantity*ActualPrice.ConvValue,2);

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
            RetailPrice.PriceType = priceRetail.PriceType;
        }
    }
}