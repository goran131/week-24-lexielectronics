using System.Threading.Tasks;
using LexiElectronics.Models;
namespace LexiElectronics.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentResult> ProcessPaymentAsync(Order order)
        { 
            await Task.CompletedTask; 

            return new PaymentResult { Success = true, TransactionId = "TEST", ErrorMessage = null };

        }
    } 
}
