using LexiElectronics.Models;

namespace LexiElectronics.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(Order order);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
