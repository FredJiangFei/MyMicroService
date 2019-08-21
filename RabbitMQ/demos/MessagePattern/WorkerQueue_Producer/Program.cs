using System;
using RabbitMQ.Client;

namespace RabbitMQ.Examples
{
    public class Program
    {
        private static IModel _model;
        private const string QueueName = "WorkerQueue_Queue";

        static void Main()
        {
            CreateConnection();

            var payment1 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1234123412341234"
            };

            var payment2 = new Payment
            {
                AmountToPay = 5.0m,
                CardNumber = "99999999"
            };
            SendMessage(payment1);
            SendMessage(payment2);

            Console.ReadLine();
        }

        private static void CreateConnection()
        {
            _model = _model = RabbitHelper.GetChannel();
            _model.QueueDeclare(QueueName, true, false, false, null);
        }

        private static void SendMessage(Payment message)
        {                        
            _model.BasicPublish("", QueueName, null, message.Serialize());
            Console.WriteLine(" Payment Sent {0}, £{1}", message.CardNumber, message.AmountToPay);                
        }
    }
}
