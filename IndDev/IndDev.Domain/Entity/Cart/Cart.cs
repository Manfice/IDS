using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Entity.Cart
{
    public class Cart
    {
        private readonly List<CartItem> _cartItems = new List<CartItem>();
        public IEnumerable<CartItem> CartItems => _cartItems;

        private static Price GetActualPrice(Product product, int quantity)
        {
            var p = new Price();
            foreach (var source in product.Prices.Where(price1 => price1.Value > 0).Where(source => quantity >= source.QuanttityFrom))
            {
                p = source;
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
                cItem.ActualPrice = GetActualPrice(product, quantity);
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
            return _cartItems.Sum(item => item.Quantity*item.ActualPrice.Value);
        }

        public void ClearList()
        {
            _cartItems.Clear();
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public Price ActualPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Quantity*ActualPrice.Value*ActualPrice.Currency.Curs;
    }
}