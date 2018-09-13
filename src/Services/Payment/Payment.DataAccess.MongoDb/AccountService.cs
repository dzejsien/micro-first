using System;
using System.Threading.Tasks;
using Core.Messages.Commands.Payments;
using Core.Messages.Events.Pricing;
using MongoDB.Driver;
using Payment.Domain;

namespace Payment.DataAccess.MongoDb
{
    public class AccountService : IAccountService
    {
        // todo: inject
        private readonly PaymentContext _context = new PaymentContext();

        public async Task<string> CreatePolicyAccountAsync(ContributionCalculatedEvent @event)
        {
            var newAccount = new Account(@event.PolicyId, (int)@event.Contribution);
            await _context.Accounts.InsertOneAsync(newAccount);
            return newAccount.Number;
        }

        public async Task SendRemittanceAsync(SendRemittanceCommand command)
        {
            Account account = await GetAccountAsync(command.PolicyId, command.AccountNumber);

            if (account == null)
            {
                throw new ArgumentException($"Account for policyId: {command.PolicyId} and accountNumber: {command.AccountNumber} doesn't exist.");
            }

            var newRemittance = new Remittance(command.AccountNumber, command.Value);
            await _context.Remittances.InsertOneAsync(newRemittance);

            await IncAccountBalanceAsync(command.AccountNumber, -command.Value);
        }

        private async Task<Account> IncAccountBalanceAsync(string accountNumber, decimal value)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.Number, accountNumber);
            var update = Builders<Account>.Update.Inc(x => x.Balance, value);
            return await _context.Accounts.FindOneAndUpdateAsync<Account>(filter, update);
        }

        public async Task<decimal> GetBalanceAsync(long policyId, string accountNumber)
        {
            var account = await GetAccountAsync(policyId, accountNumber);

            if (account == null)
            {
                throw new ArgumentException($"Account for policyId: {policyId} and accountNumber: {accountNumber} doesn't exist.");
            }

            return account.Balance;
        }

        private async Task<Account> GetAccountAsync(long policyId, string accountNumber)
        {
            return await _context.Accounts.Find(Builders<Account>.Filter.And(
                Builders<Account>.Filter.Eq(x => x.PolicyId, policyId),
                Builders<Account>.Filter.Eq(x => x.Number, accountNumber)
                )).FirstOrDefaultAsync();
        }
    }
}
