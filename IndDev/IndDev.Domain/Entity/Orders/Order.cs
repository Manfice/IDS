using System;
using System.Collections.Generic;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Entity.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Submit { get; set; }
        public string Notes { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }

    public class OrderLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal CursOnDate { get; set; }
        public string Note { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }

    public class DeliveryType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }
        public decimal FreeFrom { get; set; }
        public string Description { get; set; }
    }

    public class Delivery
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string To { get; set; }
        public string Comment { get; set; }
        public virtual DeliveryType DeliveryType { get; set; }
    }

}