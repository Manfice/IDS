using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Entity
{
    public class ImageManager
    {
         
    }

    public class UserPhoto
    {
        public int Id { get; set; }
        public byte[] PhotoData { get; set; }
        public string PhotoType { get; set; }
        public string FileName { get; set; }
        public int PhotoSise { get; set; }  
    }

    public class GoodsImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }
        public string FileName { get; set; }
        public int ImageSize { get; set; }
        public virtual Product ProductOf { get; set; }
    }
}