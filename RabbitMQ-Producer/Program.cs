using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://uaujyisy:H3i_uCahwKuNuShYsQa6TdDHfY4Cr6rF@tiger.rmq.cloudamqp.com/uaujyisy");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("message-fanout", ExchangeType.Fanout, true);

            for (int i = 1; i <= 50; i++)
            {
                string message = $"{i}. Message";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("message-fanout","",null,messageBody);
                Console.WriteLine($"Sent message : {message}");
            }

            Console.ReadLine();
        }
    }
}
