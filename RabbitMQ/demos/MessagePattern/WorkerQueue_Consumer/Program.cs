using System;
using RabbitMQ.Client;

namespace RabbitMQ.Examples
{
    public class Program
    {
        private const string QueueName = "WorkerQueue_Queue";

        static void Main()
        {
            Receive();
            Console.ReadLine();
        }

        public static void Receive()
        {
            var factory = RabbitHelper.GetFactory();
            using (var connection = factory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {
                    model.QueueDeclare(QueueName, true, false, false, null);
                    model.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(model);
                    model.BasicConsume(QueueName, false, consumer); 

                    while (true)
                    {
                        var ea = consumer.Queue.Dequeue();
                        var message = (Payment)ea.Body.DeSerialize(typeof(Payment));
                        model.BasicAck(ea.DeliveryTag, false);

                        Console.WriteLine("----- Payment Processed {0} : {1}", message.CardNumber, message.AmountToPay);
                    }
                }
            }
        }
    }
}
