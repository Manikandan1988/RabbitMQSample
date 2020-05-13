using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSendMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RabbitMQ Send Message:");
            Console.WriteLine("Do you want to sent message? Press 'Y' to contineue");
            while (Console.ReadLine() == "Y")
            {
                Console.WriteLine("Please enter the message here do you want to sent:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                string inputMessage = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connnection = factory.CreateConnection())
                {
                    using (var channel = connnection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "SampleQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var bodyMessage = Encoding.UTF8.GetBytes(inputMessage);

                        channel.BasicPublish(exchange: "", routingKey: "SampleQueue", basicProperties: null, body: bodyMessage);
                        
                        Console.WriteLine("Message Sent {0}", Encoding.UTF8.GetString(bodyMessage));
                    }
                }
                Console.WriteLine("Do you want to sent message again? Press 'Y' to contineue");
            }

        }

       
    }
}
