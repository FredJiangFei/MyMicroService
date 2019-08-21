using System;
using RabbitMQ.Client;

namespace RabbitMQ.Examples
{
    class Program
    {
        private static IModel _model;
        private const string ExchangeName = "DirectRouting_Exchange";
        private const string CardPaymentQueueName = "CardPaymentDirectRouting_Queue";
        private const string PurchaseOrderQueueName = "PurchaseOrderDirectRouting_Queue";

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
          
            var purchaseOrder1 = new PurchaseOrder
            {
                AmountToPay = 50.0m,
                CompanyName = "Company A",
                PaymentDayTerms = 75,
                PoNumber = "123434A"
            };
            var purchaseOrder2 = new PurchaseOrder
            {
                AmountToPay = 150.0m,
                CompanyName = "Company B",
                PaymentDayTerms = 75,
                PoNumber = "193434B"
            };
           
            CreateConnection();

            SendPayment(payment1);
            SendPayment(payment2);

            SendPurchaseOrder(purchaseOrder1);
            SendPurchaseOrder(purchaseOrder2);
        }

        private static void SendPayment(Payment payment)
        {
            SendMessage(payment.Serialize(), "CardPayment");
            Console.WriteLine(" Payment Sent {0}, £{1}", payment.CardNumber, payment.AmountToPay); 
        }

        private static void SendPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            SendMessage(purchaseOrder.Serialize(), "PurchaseOrder");
            Console.WriteLine(" Purchase Order Sent {0}, £{1}, {2}, {3}", purchaseOrder.CompanyName, purchaseOrder.AmountToPay, purchaseOrder.PaymentDayTerms, purchaseOrder.PoNumber); 
        }

        private static void CreateConnection()
        {
            _model = _model = RabbitHelper.GetChannel();
            _model.ExchangeDeclare(ExchangeName, "direct");
           
            _model.QueueDeclare(PurchaseOrderQueueName, true, false, false, null);
            _model.QueueBind(PurchaseOrderQueueName, ExchangeName, "PurchaseOrder");

            _model.QueueDeclare(CardPaymentQueueName, true, false, false, null);
            _model.QueueBind(CardPaymentQueueName, ExchangeName, "CardPayment");
        }

        private static void SendMessage(byte[] message, string routingKey)
        {                       
            _model.BasicPublish(ExchangeName, routingKey, null, message);          
        }
    }
}
