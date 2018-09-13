using Core.Common.EventBus;
using Newtonsoft.Json;

namespace Core.Messages.Commands.Payments
{
    public class CreatePolicyAccountCommand : ICommand
    {
        [JsonConstructor]
        public CreatePolicyAccountCommand(int charge, string number, long policyId)
        {
            Charge = charge;
            Number = number;
            PolicyId = policyId;
        }

        public int Charge { get; set; }
        public string Number { get; set; }
        public long PolicyId { get; set; }
    }
}
