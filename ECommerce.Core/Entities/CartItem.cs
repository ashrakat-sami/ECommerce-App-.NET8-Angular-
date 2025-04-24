namespace ECommerce.Core.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string image { set; get; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
    }
}