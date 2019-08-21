using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace RPCDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpclient = new TcpClient();
                Console.WriteLine("Connecting..");
                tcpclient.Connect("127.0.0.1", 8000);

                Console.WriteLine("Ente the String you want to send ");
                string str = Console.ReadLine();

                Stream stm = tcpclient.GetStream();
                ASCIIEncoding ascnd = new ASCIIEncoding();
                byte[] ba = ascnd.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }

                tcpclient.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.StackTrace);
            }
        }
    }
}
