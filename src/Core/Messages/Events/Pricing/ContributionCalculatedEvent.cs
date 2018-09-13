using Core.Common.EventBus;

namespace Core.Messages.Events.Pricing
{
    public class ContributionCalculatedEvent : IEvent
    {
        public ContributionCalculatedEvent(long policyId, decimal contribution)
        {
            PolicyId = policyId;
            Contribution = contribution;
        }

        public long PolicyId { get; set; }
        public decimal Contribution { get; set; }
    }
}
