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

    public class ProductPhoto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string AltText { get; set; }
        public PhotoType PhotoType { get; set; }
        public virtual Product Product { get; set; }
    }

    public enum PhotoType
    {
        Avatar,Photo
    }
}