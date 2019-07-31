using BinanceLibAPI.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace BinanceLibAPI.RabbitMQ
{
    public class Publisher
    {
        private ConnectionFactory _cf;
        private IConnection _conn;
        private IModel _channel;
        private IModel _Messagechannel;
        private string messageActionQueueName;
        private string queueName;
        private string exchangeName;

        public Publisher(string hostname, int port, string exchangeName, string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            try
            {
                this.queueName = queueName;
                this.exchangeName = exchangeName;
                messageActionQueueName = "message" + queueName;
                _cf = new ConnectionFactory() { HostName = RabbitmqConfiguration.HOST };
                _conn = _cf.CreateConnection();
                _channel = _conn.CreateModel();
                _channel.QueueDeclare(queue: queueName,
                                     durable: durable,
                                     exclusive: exclusive,
                                     autoDelete: autoDelete,
                                     arguments: arguments);

                _channel.QueueBind(queue: messageActionQueueName,
                              exchange: exchangeName,
                              routingKey: messageActionQueueName);
                _channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
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
                StreamMessage streamMessage = JsonConvert.DeserializeObject<StreamMessage>(message);
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(streamMessage.data));
                _channel.BasicPublish(exchange : exchangeName,
                                     routingKey : queueName,
                                     basicProperties: null,
                                     body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
