using Core.Common.EventBus;
using System.Collections.Generic;

namespace Core.Messages.Events.Policies
{
    public class PolicyCreatedEvent : IEvent
    {
        public PolicyCreatedEvent(long policyId, IList<string> productCodes)
        {
            PolicyId = policyId;
            ProductCodes = productCodes;
        }

        public long PolicyId { get; set; }
        public IList<string> ProductCodes { get; set; }
    }
}
