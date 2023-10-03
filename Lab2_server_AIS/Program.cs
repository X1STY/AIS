using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;


namespace Lab2_Client_AIS
{
    public class Program
    {
        static IniFile cfg = new IniFile(@"C:\Users\X1STY-\source\repos\X1STY\AIS\config.ini");
        
        private static int CLIENT_PORT = Int32.Parse(cfg.Read("CLIENT_PORT", "AIS"));
        private static int SERVER_PORT = Int32.Parse(cfg.Read("SERVER_PORT", "AIS"));
        private static string SERVER_IP = cfg.Read("IP", "AIS");

        static UdpClient udpClient;

        private static void StartClient()
        {

            while (true)
            {
                RecieveMessage();
                //ConsoleKey option = Console.ReadKey().Key;
                
                SendMessage(Console.ReadLine());

                RecieveMessage();
            }
        }

        private static void RecieveMessage()
        {
            var remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIP);
                string message = Encoding.Unicode.GetString(data);
                Console.WriteLine(message);
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private static void SendMessage(string msg)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(msg);
                udpClient.Send(data, data.Length, SERVER_IP, SERVER_PORT);
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static void Main(string[] args)
        {
            udpClient = new UdpClient(CLIENT_PORT);
            StartClient();
        }
    }
}
