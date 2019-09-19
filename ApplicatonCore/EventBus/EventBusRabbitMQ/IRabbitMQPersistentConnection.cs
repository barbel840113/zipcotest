using RabbitMQ.Client;
using System;

namespace EventBusRabbitMQLibrary
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}