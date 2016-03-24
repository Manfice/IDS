namespace IndDev.Domain.Entity.Products
{
    public class Brand
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}