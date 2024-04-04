using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HW_Client_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 11111);

                Stream stream = tcpClient.GetStream();
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteSend = null;
                byte[] byteRecv = new byte[1024];
                int bytes = 0;

                Console.WriteLine("Enter your name:\n");
                string msg = Console.ReadLine();
                byteSend = encoding.GetBytes(msg);
                stream.Write(byteSend, 0, byteSend.Length);

                while (true)
                {
                    Console.WriteLine("Enter what exchange rate you want to know(Example: UAH EURO)\nFor now we contain UAH, USD, EURO, KZT:\n");
                    msg = Console.ReadLine();
                    byteSend = encoding.GetBytes(msg);
                    stream.Write(byteSend,0, byteSend.Length);

                    bytes = stream.Read(byteRecv, 0, byteRecv.Length);
                    Console.WriteLine("Answer: " + Encoding.UTF8.GetString(byteRecv, 0, bytes));
                    if (msg.ToUpper() == "EXIT")
                    {
                        break;
                    }
                }
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
