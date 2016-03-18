using System;

namespace IndDev.Domain.Context
{ 
    public class Curency
    {
        public int Id { get; set; }
        public DateTime ReqDate { get; set; }
        public string Title { get; set; }
        public string Stitle { get; set; }
        public decimal CurValue { get; set; }
    }
}
