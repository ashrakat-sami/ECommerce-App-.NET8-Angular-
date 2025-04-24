namespace ECommerce.Core.Entities.Order
{
    public class OrderItem:BaseEntity<int>
    {
        public OrderItem()
        {
        }

        public OrderItem(int productItemId, int quantity, decimal price, string productName, string mainImage)
        {
            ProductItemId = productItemId;
            Quantity = quantity;
            Price = price;
            ProductName = productName;
            MainImage = mainImage;
        }

        public int ProductItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string MainImage { get; set; }
    
    }
}