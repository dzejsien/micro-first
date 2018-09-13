using Core.Common.EventBus;
using RawRabbit.Context;
using System;

namespace Core.EventBus
{
    public class CorrelationContext : IMessageContext
    {
        public CorrelationContext()
        {

        }

        public CorrelationContext(Guid id, Guid userId, Guid resourceId, string name, string origin, string culture, string resource)
        {
            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            Name = name;
            Origin = origin;
            Culture = culture;
            Resource = resource;
        }

        public Guid GlobalRequestId { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ResourceId { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Culture { get; set; }
        public string Resource { get; set; }

        public static CorrelationContext Create<T>(Guid id, Guid userId, Guid resourceId, string origin, string culture, string resource = "")
        {
            return new CorrelationContext(id, userId, resourceId, typeof(T).Name, origin, culture, resource);
        }
    }
}
