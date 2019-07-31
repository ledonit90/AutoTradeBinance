using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceLibAPI.RabbitMQ
{
    public class Subscriber
    {
        private ConnectionFactory _cf;
        private IConnection _conn;
        private IModel _channel;
        private string queueName;
        private string exchangeName;

        public Subscriber(string hostname, int port, string exchangeName, string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            this.queueName = queueName;
            this.exchangeName = exchangeName;
            _cf = new ConnectionFactory() { HostName = hostname, Port = port };
            _conn = _cf.CreateConnection();

            _channel.QueueDeclare(queue: queueName,
                                 durable: durable,
                                 exclusive: exclusive,
                                 autoDelete: autoDelete,
                                 arguments: arguments);
        }

        public IModel getChannel()
        {
            return _conn.CreateModel();
        }

        public void CloseChannel(IModel channel)
        {
            channel.Close();
        }

        public void SubcribeAChannel()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
