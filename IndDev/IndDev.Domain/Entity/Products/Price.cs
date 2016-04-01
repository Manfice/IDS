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
    }

    public enum PriceType
    {
        Opt,LowOpt,Retail,Sale
    }
}