using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Remibit.Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remibit.Utility.RabittMQ
{
    public class Subscriber
    {
        private ConnectionFactory _cf;
        private IConnection _conn;
        private IModel _channel;

        public string RABBITMQ_IP { get; set; }
        public int RABBIT_PORT { get; set; }
        public string RABBITMQ_USERNAME { get; set; }
        public string RABBITMQ_PASSWORD { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public bool durable { get; set; } = true;
        public bool exclusive { get; set; } = false;
        public bool autoDelete { get; set; } = false;
        public IDictionary<string, object> arguments { get; set; } = null;

        public Subscriber()
        {
        }

        public void getStartedUse()
        {
            _cf = new ConnectionFactory() { HostName = AppConstConfig.RABBITMQ_IP, Port = AppConstConfig.RABBIT_PORT, UserName = AppConstConfig.RABBITMQ_USERNAME, Password = AppConstConfig.RABBITMQ_PASSWORD };
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

        public void SubcribeAsync(EventHandler<BasicDeliverEventArgs> hamgansau)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += hamgansau;

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
