using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ_Producer
{
    class Program
    {
        public enum LogNames
        {
            Critical=1,
            Danger=2,
            Info=3,
            Warning=4
        }
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://uaujyisy:H3i_uCahwKuNuShYsQa6TdDHfY4Cr6rF@tiger.rmq.cloudamqp.com/uaujyisy");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-topic", ExchangeType.Topic, true);

            for (int i = 1; i <= 50; i++)
            {
                LogNames log1 = (LogNames)new Random().Next(1, 5);
                LogNames log2 = (LogNames)new Random().Next(1, 5);
                LogNames log3 = (LogNames)new Random().Next(1, 5);
                string message = $"LogType : {log1}-{log2}-{log3}";
                var routeKey = $"{log1}.{log2}.{log3}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-topic",routeKey,null,messageBody);
                Console.WriteLine($"Log gönderilmiştir : {message}");
            }

            Console.ReadLine();
        }
    }
}
