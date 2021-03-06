using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ_Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://uaujyisy:H3i_uCahwKuNuShYsQa6TdDHfY4Cr6rF@tiger.rmq.cloudamqp.com/uaujyisy");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            var queueName = channel.QueueDeclare().QueueName;
            var routeKey = "*.Critical.*";
            channel.QueueBind(queueName, "logs-topic", routeKey, null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queueName, false, consumer); //false=> Benim ack etmemi bekle.

            Console.WriteLine("Waiting for messages!");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());    
                Thread.Sleep(1000);
                Console.WriteLine("Coming Message: " + message);
                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
