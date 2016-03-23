namespace IndDev.Domain.Entity.Products
{
    public class Price
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Value { get; set; }
        public PriceType PriceType { get; set; }
        public virtual Product Product { get; set; }
        public virtual Currency Currency { get; set; }  
    }

    public enum PriceType
    {
        InputPrice,
        OutputPrice
    }
}