using System.Globalization;

namespace IndDev.Domain.Entity.Products
{
    public class Price
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public bool Publish { get; set; }
        public int QuanttityFrom { get; set; }
        public PriceType PriceType { get; set; }
        public virtual Product Product { get; set; }
        public virtual Currency Currency { get; set; }

        public decimal GetPriceRubl()
        {
            decimal val;
            var conversionValue = (Value*Currency.Curs).ToString("C");
            var nfi = new CultureInfo("ru-Ru", false).NumberFormat;
            nfi.CurrencyDecimalDigits = 2;
            decimal.TryParse(conversionValue, NumberStyles.Currency, nfi, out val);
            return val;
        }
    }

    public enum PriceType
    {
        Opt,LowOpt,Retail,Sale
    }
}