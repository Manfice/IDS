using System.Collections.Generic;
using System.Web;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Articul { get; set; }
        public string Title { get; set; }
        public double PriceIn { get; set; }
        public string Currency { get; set; }
        public double PriceOut { get; set; }
    }

    public class AddProductViewModel
    {
        public int SubCatId { get; set; }
        public string ReturnUrl { get; set; }
        public int SelBrand { get; set; }
        public int SelMU { get; set; }
        public IEnumerable<MesureUnit> MesureUnits { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public Product Product { get; set; }
    }

    public class AddPhotoViewModel
    {
        public int ProductId { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}