using Core.Messages.Commands.Payments;
using Core.Messages.Events.Pricing;
using System.Threading.Tasks;

namespace Payment.Domain
{
    public interface IAccountService
    {
        Task<string> CreatePolicyAccountAsync(ContributionCalculatedEvent @event);
        Task SendRemittanceAsync(SendRemittanceCommand command);
        Task<decimal> GetBalanceAsync(long policyId, string accountNumber);
    }
}
