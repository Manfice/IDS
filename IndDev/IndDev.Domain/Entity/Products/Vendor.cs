using System.Collections.Generic;

namespace IndDev.Domain.Entity.Products
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Brand> Brands { get; set; }    
    }
}