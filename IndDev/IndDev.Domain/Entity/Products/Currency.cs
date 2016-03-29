using System;

namespace IndDev.Domain.Entity.Products
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string StringCode { get; set; }
        public DateTime ActualDate { get; set; }
        public decimal Curs { get; set; }

    }
}