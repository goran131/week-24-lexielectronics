namespace LexiElectronics.Models.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Price { get; set; }
        public int Quantity { get; set; }       
        public decimal TotalSum => Price * Quantity;

        public CartItemDto() { }

        public CartItemDto(int productId, string productName, int price, int quantity) 
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.Price = price;
            this.Quantity = quantity;
        }
    }
}
