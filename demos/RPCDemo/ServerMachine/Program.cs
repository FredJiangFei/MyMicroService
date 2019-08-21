using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
                TcpListener mylist = new TcpListener(ipaddress, 8000);
                mylist.Start();
                Console.WriteLine("Waiting for Connections...");

                Socket socket = mylist.AcceptSocket();
                Console.WriteLine("Connection Accepted From:" + socket.RemoteEndPoint);

                byte[] b = new byte[100];
                int k = socket.Receive(b);
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(b[i]));
                }

                ASCIIEncoding asencd = new ASCIIEncoding();
                socket.Send(asencd.GetBytes("Message Received!"));
                socket.Close();
                mylist.Stop();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error.." + ex.StackTrace);
            }
        }
    }
}
