using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQ.Examples
{
   public class RabbitHelper
    {
        public static IModel GetChannel()
        {
            var factory = GetFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            return channel;
        }

        public static ConnectionFactory GetFactory()
        {
            return new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "1qaz2wsx3edc4rfv",
                VirtualHost = "rabbitmq_vhost"
            };
        }
    }
}
