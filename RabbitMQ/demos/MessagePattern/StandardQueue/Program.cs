using System;
using RabbitMQ.Client;

namespace RabbitMQ.Examples
{
    class Program
    {
        private static IModel _model;
        private const string QueueName = "StandardQueue_ExampleQueue";

        public static void Main()
        {
            CreateQueue();

            var payment = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1234123412341234",
                Name = "Mr S Haunts"
            };
            SendMessage(payment);
                                 
            Recieve();

            Console.ReadLine();
        }

        private static void CreateQueue()
        {
            _model = RabbitHelper.GetChannel();
            _model.QueueDeclare(QueueName, true, false, false, null);            
        }

        private static void SendMessage(Payment message)
        {            
            _model.BasicPublish("", QueueName, null, message.Serialize());
            Console.WriteLine("Payment Sent : {0} : {1} : {2}", message.CardNumber, message.AmountToPay, message.Name);            
        }

        public static void Recieve()
        {
            var consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(QueueName, true, consumer);

            var msgCount = GetMessageCount(_model, QueueName);
            var count = 0;
            while (count < msgCount)
            {                               
                var ea = consumer.Queue.Dequeue();
                var message = (Payment)ea.Body.DeSerialize(typeof(Payment));
                Console.WriteLine("----- Received {0} : {1} : {2}", message.CardNumber, message.AmountToPay, message.Name);
                count++;
            }                   
        }

        private static uint GetMessageCount(IModel channel, string queueName)
        {
            var results = channel.QueueDeclare(queueName, true, false, false, null);
            return results.MessageCount;                
        }
    }
}
