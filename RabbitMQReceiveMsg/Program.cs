using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQReceiveMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            string queueName = "SampleQueue";
            var rabbitMqConnection = factory.CreateConnection();
            var rabbitMqChannel = rabbitMqConnection.CreateModel();

            rabbitMqChannel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            rabbitMqChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            int messageCount = Convert.ToInt16(rabbitMqChannel.MessageCount(queueName));
            Console.WriteLine(" Listening to the queue. This channels has {0} messages on the queue", messageCount);

            var consumer = new EventingBasicConsumer(rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Message received: " + message);
                rabbitMqChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                Thread.Sleep(1000);
            };
            rabbitMqChannel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            Thread.Sleep(1000 * messageCount);
            Console.WriteLine(" Connection closed, no more messages.");
            Console.ReadLine();
            //var factory = new ConnectionFactory() { HostName = "localhost" };

            //using (var connnection = factory.CreateConnection())
            //{
            //    using (var channel = connnection.CreateModel())
            //    {
            //        channel.QueueDeclare(queue: "SampleQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            //        var consumer = new EventingBasicConsumer(channel);
            //        consumer.Received += (model, ea) =>
            //        {
            //            var body = ea.Body;
            //            var message = Encoding.UTF8.GetString(body.ToArray());
            //            Console.WriteLine(" [x] Received {0}", message);
            //        };
            //        channel.BasicConsume(queue: "SampleQueue",
            //                             autoAck: true,
            //                             consumer: consumer);
            //    }

            //}
            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();

        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var receivedMsg = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine("Message Received {0}", receivedMsg);
        }
    }
}
