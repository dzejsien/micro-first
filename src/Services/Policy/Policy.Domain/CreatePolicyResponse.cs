using Core.Common.EventBus;

namespace Policy.Domain
{
    public class CreatePolicyResponse : IResponse
    {
        public long PolicyId { get; set; }
    }
}
