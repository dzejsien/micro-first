using Refit;
using System.Threading.Tasks;

namespace MicroDemo.Api.Api
{
    public interface IPaymentApi
    {
        [Get("/v1/payments/{policyId}/accounts/{accountNumber}")]
        Task<decimal> GetAsync(long policyId, string accountNumber);
    }
}
