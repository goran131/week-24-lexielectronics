using LexiElectronics.Data;
using LexiElectronics.Models.DTOs;

namespace LexiElectronics.Models.DTOs
{
    public class ShoppingCartDto 
    {
        public List<CartItemDto> CartItems { get; set; }

        public decimal TotalSum => CartItems.Sum(i => i.Price * i.Quantity); 

        public ShoppingCartDto() {
            CartItems = new List<CartItemDto>();
        }       
    }
}
