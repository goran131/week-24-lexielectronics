using LexiElectronics.Models;

namespace LexiElectronics.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IConfiguration _config;

        public StripePaymentService(IConfiguration config)
        {
            _config = config;
        }

        public Task<PaymentResult> ProcessPaymentAsync(Order order)
        {
            // Placeholder: integrate Stripe.NET here using API keys from configuration.
            // For testing, return success.
            return Task.FromResult(new PaymentResult { Success = true, TransactionId = "test_txn_123" });
        }
    }
}
