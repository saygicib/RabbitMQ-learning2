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

            channel.ExchangeDeclare("logs-direct", ExchangeType.Direct, true);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x => {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false);
                channel.QueueBind(queueName, "logs-direct", routeKey, null);
            });

            for (int i = 1; i <= 50; i++)
            {
                LogNames log = (LogNames)new Random().Next(1, 5);
                string message = $"LogType : {log}";
                var routeKey = $"route-{log}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-direct",routeKey,null,messageBody);
                Console.WriteLine($"Log gönderilmiştir : {message}");
            }

            Console.ReadLine();
        }
    }
}
