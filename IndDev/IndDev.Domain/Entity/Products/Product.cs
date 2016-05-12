using System.Collections.Generic;
using IndDev.Domain.Entity.Menu;

namespace IndDev.Domain.Entity.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Warning { get; set; }
        public string Articul { get; set; }
        public bool IsService { get; set; } 
        public string Warranty { get; set; }
        public bool Reclama { get; set; }
        public decimal Rate { get; set; }
        public virtual MesureUnit MesureUnit { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ProductMenuItem Categoy { get; set; }
        public virtual Menu.Menu MenuItem { get; set; }
        public virtual Stock.Stock Stock { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; } 
    }
}