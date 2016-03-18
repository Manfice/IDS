using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExternalRes.Enity
{
    public class Currency
    {
        public DateTime ReqDate { get; set; }
        public string Title { get; set; }
        public string Stitle { get; set; }
        public decimal CurValue { get; set; }
    }
}
