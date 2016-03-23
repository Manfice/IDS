namespace IndDev.Domain.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double PriceIn { get; set; }
        public string Currency { get; set; }
        public double PriceOut { get; set; }
    }
}