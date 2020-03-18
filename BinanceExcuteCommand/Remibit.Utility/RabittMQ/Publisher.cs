using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Remibit.Utility.RabitMQ
{
    public class Publisher
    {
        private ConnectionFactory _cf;
        private IConnection _conn;
        private IModel _channel;
        private string messageActionQueueName;
        private string queueName;
        private string exchangeName;

        public Publisher(string exchangeName = RabbitConfig.EXCHANGENAME, string queueName = RabbitConfig.QUEUENAME, bool durable = true, bool exclusive = false, bool autoDelete = false, string hostname = RabbitConfig.HOST, int port = RabbitConfig.Port, IDictionary<string, object> arguments = null)
        {
            try
            {
                this.queueName = queueName;
                this.exchangeName = exchangeName;
                messageActionQueueName = "message" + queueName;
                _cf = new ConnectionFactory() { HostName = RabbitConfig.HOST, UserName = "muabanaltcoinnhe", Password = "Levandon_90" };
                _conn = _cf.CreateConnection();
                _channel = _conn.CreateModel();
                _channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
                _channel.QueueDeclare(queue: queueName,
                                     durable: durable,
                                     exclusive: exclusive,
                                     autoDelete: autoDelete,
                                     arguments: arguments);

                _channel.QueueBind(queue: messageActionQueueName,
                              exchange: exchangeName,
                              routingKey: messageActionQueueName);

                ReceiveMessageAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void ReceiveMessageAction()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Receive Message {0}", message);
            };

            _channel.BasicConsume(queue: "message" + queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void CloseChannel(IModel channel)
        {
            channel.Close();
        }

        public void sendAMessage(string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                _channel.BasicPublish(exchange: exchangeName,
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void PublishMessage(string message)
        {
                _channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
        }
    }
}
