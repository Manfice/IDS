using System.Collections.Generic;
using System.Web;
using IndDev.Domain.Context;
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

    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<MesureUnit> MesureUnits { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<Price> Prices { get; set; } 
        public int SelBr { get; set; }
        public int SelVr { get; set; }
        public int SelMu { get; set; }
    }

    public class PriceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Currency Currency { get; set; }
        public decimal Value { get; set; }
        public decimal ConvValue { get; set; }
        public Product Product { get; set; }
    }

    public class PriceSetter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PriceTp { get; set; }
        public decimal Value { get; set; }
        public bool Public { get; set; }
        public int SelCurr { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
        public IEnumerable<PriceType> PriceTypes { get; set; }
        public PriceViewModel PriceViewModel { get; set; }

    }

    public class AddPhotoViewModel
    {
        public int ProductId { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}