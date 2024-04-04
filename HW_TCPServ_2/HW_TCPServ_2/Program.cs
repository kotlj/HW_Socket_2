using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HW_TCPServ_2
{
    internal class TcpServ
    {
        static IPAddress locIp = IPAddress.Parse("127.0.0.1");
        const int port = 11111;
        static void Main(string[] args)
        {
            Dictionary<string, double> rate = new Dictionary<string, double>()
            {
                {"UAH", 4.0 },
                {"USD", 40.0 },
                {"EURO", 100.0 },
                {"KZT", 0.1 }
            };

            TcpListener serv = new TcpListener(locIp, port);
            serv.Start();
            Console.WriteLine("Serv started");

            Socket socket = serv.AcceptSocket();
            Console.WriteLine("Connected");
            StringBuilder sb = new StringBuilder();

            try
            {
                byte[] bytesRec = new byte[1024];
                int bytes = socket.Receive(bytesRec, bytesRec.Length, 0);
                string dataRec = Encoding.UTF8.GetString(bytesRec, 0, bytes);

                Console.WriteLine($"{dataRec}\n");
                Console.WriteLine($"Connected: {DateTime.Now.ToString()}\n");
                sb.Append($"{dataRec}\n");
                sb.Append($"Connected: {DateTime.Now.ToString()}");
                sb.Append('\n');

                while (true)
                {
                    bytes = socket.Receive(bytesRec, bytesRec.Length, 0);
                    dataRec = Encoding.UTF8.GetString(bytesRec, 0, bytes);
                    dataRec = dataRec.ToUpper();
                    string resp = "";
                    if (dataRec != "EXIT")
                    {
                        sb.Append(dataRec);
                        sb.Append('\n');
                        try
                        {
                            string[] split = dataRec.Split(' ');
                            Console.WriteLine(split[0] + ' ' + split[1]);
                            resp = $"{rate[split[0]] / rate[split[1]]}";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            resp = "ERROR: invalid format";
                        }
                    }
                    else
                    {
                        resp = "disconnected";
                        sb.Append($"Disconnected: {DateTime.Now.ToString()}");
                        Console.WriteLine($"Disconnected: {DateTime.Now.ToString()}");
                        socket.Send(Encoding.UTF8.GetBytes(resp));
                        break;
                    }
                    
                    socket.Send(Encoding.UTF8.GetBytes(resp));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                sb.Append('\n');
                File.WriteAllText("Log.txt" ,sb.ToString());
                socket.Close();
                serv.Stop();
            }
        }
    }
}
