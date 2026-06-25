using System.ComponentModel.DataAnnotations.Schema;

namespace LexiElectronics.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }

        [NotMapped]
        public Order Order { get; set; }

        public int ProductId { get; set; }

        [NotMapped]
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        [NotMapped]
        public int Sum { get; set; }

        public OrderItem() { }

        public OrderItem(int orderId, int productId, int quantity, int price) 
        {
            this.OrderId = orderId;
            this.ProductId = productId;
            this.Quantity = quantity;
            this.Price = price;
            this.Sum = Quantity * Price;
        }
    }
}
