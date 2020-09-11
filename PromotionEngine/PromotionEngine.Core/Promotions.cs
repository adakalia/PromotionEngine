namespace PromotionEngine.Core
{
    public class Promotions
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public char PrimarySku { get; set; }
        public char SecondarySku { get; set; }

    }
}
