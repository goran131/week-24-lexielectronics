using LexiElectronics.Models;
using LexiElectronics.Data;
using LexiElectronics.Models.DTOs;
namespace LexiElectronics.Models.ViewModels
{
    public class CheckoutVM
    {
        private Order order { get; set; }

        public ApplicationUser User { get; set; }
        public ShoppingCartDto cartDto { get; set; }

        public bool DeliveryAddressExists { get; set; }

        public string ReturnUrl { get; set; }

        public int TotalSum => cartDto != null ? cartDto.CartItems.Sum(i => i.Price * i.Quantity) : 0;

        public int FreightSum { get; set; }

        public int TotalSumToPay { get; set; }

        public decimal Moms => (TotalSumToPay * 25)/100;

        public bool PaymentResult { get; set; }

        public CheckoutVM() {}
        public CheckoutVM(ShoppingCartDto cart)
        {
            this.cartDto = cart;
        }
    }
}
