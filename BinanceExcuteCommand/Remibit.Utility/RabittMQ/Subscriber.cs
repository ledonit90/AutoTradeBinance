using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Remibit.Models.SupportObj;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace Remibit.Utility.RabitMQ
{
    public class Subscriber
    {
        private ConnectionFactory _cf;
        private IConnection _conn;
        private IModel _channel;
        private IHubContext<PriceHub, IPriceHub> _priceHub;

        public string QueueName { get ; set ; }
        public string ExchangeName { get; set ; }
        public bool durable { get; set ; } = true;
        public bool exclusive { get; set ; } = false;
        public bool autoDelete { get; set; } = false;
        public IDictionary<string, object> arguments { get; set; } = null;

        public Subscriber(IHubContext<PriceHub, IPriceHub> pricehub)
        {
            _priceHub = pricehub;
        }

        public void getStartedUse()
        {
            _cf = new ConnectionFactory() { HostName = RabbitConfig.HOST, Port = RabbitConfig.Port, UserName = RabbitConfig.UserName, Password = RabbitConfig.Password };
            _conn = _cf.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: "topic");
            _channel.QueueDeclare(queue: QueueName,
                                 durable: durable,
                                 exclusive: exclusive,
                                 autoDelete: autoDelete,
                                 arguments: arguments);
        }

        public IModel getChannel()
        {
            return _conn.CreateModel();
        }

        public void CloseChannel()
        {
            this._channel.Close();
            this._conn.Close();
        }

        public void SubcribeAChannel()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                // Use SignalR send message
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                await _priceHub.Clients.All.SendMessageAsync(message);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(queue: QueueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }
    }
}
