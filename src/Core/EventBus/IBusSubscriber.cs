﻿using Core.Common.EventBus;

namespace Core.EventBus
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>(string exchangeName = null) where TCommand : ICommand;
        IBusSubscriber SubscribeEvent<TEvent>(string exchangeName = null) where TEvent : IEvent;
    }
}
