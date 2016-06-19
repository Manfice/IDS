using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Entity.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime OrderDate { get; set; }
        public string Notes { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }

        public decimal CalcTotalSumm()
        {
            var summ = decimal.Round(OrderLines.Sum(line => line.Price * line.Quantity), 2);
            summ = summ + Delivery.DeliveryCost;
            return summ;
        }

        public decimal CalcNds()
        {
            return decimal.Round(CalcTotalSumm()*18/118, 2);
        }
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

        public decimal TotalSumm => Quantity*Price;
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
        public decimal DeliveryCost { get; set; }
        public virtual DeliveryType DeliveryType { get; set; }
    }

    public class OrderStatus
    {
        public int Id { get; set; }
        public bool Moderated { get; set; }
        public bool Paid { get; set; }
        public bool UnderDelivery { get; set; }
        public virtual DeliveryData DeliveryData { get; set; }
    }

    public class DeliveryData
    {
        public int Id { get; set; }
        public string ShipmentCompany { get; set; }
        public string Number { get; set; }
        public DateTime DeliveryDate { get; set; }

    }
}