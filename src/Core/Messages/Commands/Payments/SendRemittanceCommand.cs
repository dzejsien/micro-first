using Core.Common.EventBus;
using Newtonsoft.Json;

namespace Core.Messages.Commands.Payments
{
    public class SendRemittanceCommand : ICommand
    {
        //[JsonConstructor]
        //public SendRemittanceCommand(int value)
        //{
        //    Value = value;
        //}

        [JsonConstructor]
        public SendRemittanceCommand(decimal value, long policyId, string accountNumber)
        {
            PolicyId = policyId;
            AccountNumber = accountNumber;
            Value = value;
        }

        public long PolicyId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Value { get; set; }
    }
}
