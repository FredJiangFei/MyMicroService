

using System;
using RabbitMQ.Client;

namespace RabbitMQ.Examples
{
    class Program
    {
        private static IModel _model;
        private const string ExchangeName = "PublishSubscribe_Exchange";

        static void Main()
        {
            var payment1 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1234123412341234"
            };
            var payment2 = new Payment
            {
                AmountToPay = 5.0m,
                CardNumber = "1234123412341234"
            };
          
            CreateConnection();

            SendMessage(payment1);
            SendMessage(payment2);

            Console.ReadLine();
        }

        private static void CreateConnection()
        {
            _model = RabbitHelper.GetChannel();
            _model.ExchangeDeclare(ExchangeName, "fanout", false);
        }

        private static void SendMessage(Payment message)
        {           
            _model.BasicPublish(ExchangeName, "", null, message.Serialize());
            Console.WriteLine(" Payment Sent {0}, £{1}", message.CardNumber, message.AmountToPay);             
        }
    }
}
