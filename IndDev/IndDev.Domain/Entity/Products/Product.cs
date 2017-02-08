using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IndDev.Domain.Entity.Menu;

namespace IndDev.Domain.Entity.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CanonicTitle { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [AllowHtml]
        public string Warning { get; set; }
        public string Articul { get; set; }
        public string ShotDescription { get; set; }
        public bool IsService { get; set; } 
        public string Warranty { get; set; }
        public bool Reclama { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Show { get; set; }
        public virtual MesureUnit MesureUnit { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ProductMenuItem Categoy { get; set; }
        public virtual Menu.Menu MenuItem { get; set; }
        public virtual Stock.Stock Stock { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }

        public Price GetOptPrice()
        {
            var p = this.Prices.FirstOrDefault(price => price.PriceType == PriceType.Sale);
            if (p!=null && p.GetPriceRubl()>0)
            {
                return p;
            }
            p = this.Prices.FirstOrDefault(price => price.PriceType == PriceType.Opt);
            return p;
        }
    }


}