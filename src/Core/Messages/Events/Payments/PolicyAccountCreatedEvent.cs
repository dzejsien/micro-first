using Core.Common.EventBus;

namespace Core.Messages.Events.Payments
{
    public class PolicyAccountCreatedEvent : IEvent
    {
        public PolicyAccountCreatedEvent(long policyId, string number)
        {
            PolicyId = policyId;
            Number = number;
        }

        public long PolicyId { get; }
        public string Number { get; }
    }
}
