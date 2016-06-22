namespace IndDev.Domain.Entity.Products
{
    public class Brand
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string BrandLink { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual BrandImage BrandImage { get; set; }
        
    }

    public class BrandImage
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string ContentType { get; set; }
    }
}