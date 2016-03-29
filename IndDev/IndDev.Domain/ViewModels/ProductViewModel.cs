using System.Collections.Generic;
using System.Web;
using IndDev.Domain.Context;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Articul { get; set; }
        public string Title { get; set; }
        public decimal PriceIn { get; set; }
        public string Currency { get; set; }
        public decimal PriceOut { get; set; }
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
        public ProductPhoto Avatar { get; set; }
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
        private decimal OriginalValue { get; set; }
        private decimal ConversionValue { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
        public Currency Currency { get; set; }

        public decimal OriginalPrice
        {
            get { return OriginalValue; }
            set
            {
                ConversionValue = value*Currency.Curs;
                OriginalValue = value;
            }
        }

        public decimal ConvValue
        {
            get { return ConversionValue; }
            set { ConversionValue = value; }
        }

        public Product Product { get; set; }
    }

    public class PriceSetter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public bool Public { get; set; }
        public int SelCurr { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
        public PriceType PriceType { get; set; }
        public PriceViewModel PriceViewModel { get; set; }

    }

    public class AddPhotoViewModel
    {
        public int ProductId { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}