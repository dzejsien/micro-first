using Core.Common.EventBus;
using RawRabbit;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.EventBus
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly IBusClient<CorrelationContext> _client;
        private readonly IServiceProvider _serviceProvider;

        public BusSubscriber(IBusClient<CorrelationContext> client, IServiceProvider serviceProvider)
        {
            _client = client;
            _serviceProvider = serviceProvider;
        }

        public IBusSubscriber SubscribeCommand<TCommand>(string exchangeName = null) where TCommand : ICommand
        {
            _client.SubscribeAsync<TCommand>(async (message, context) => {
                var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
                await handler.HandleAsync(message);
            }, cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TCommand>(exchangeName))));

            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>(string exchangeName = null) where TEvent : IEvent
        {
            _client.SubscribeAsync<TEvent>(async (message, context) => {
                var handler = _serviceProvider.GetService<IEventHandler<TEvent>>();
                await handler.HandleAsync(message);
            }, cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TEvent>(exchangeName))));

            return this;
        }

        private static string GetExchangeName<T>(string name = null)
            => string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}"
                : $"{name}/{typeof(T).Name}";
    }
}
