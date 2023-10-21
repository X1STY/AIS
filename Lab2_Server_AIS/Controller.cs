using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab2_Server_AIS
{
    public class Controller
    {
        static IniFile cfg = new IniFile(@"C:\Users\X1STY-\source\repos\X1STY\AIS\config.ini");

        private static int CLIENT_PORT = Int32.Parse(cfg.Read("CLIENT_PORT", "AIS"));
        private static int SERVER_PORT = Int32.Parse(cfg.Read("SERVER_PORT", "AIS"));
        private static string CLIENT_IP = cfg.Read("IP", "AIS");

        static UdpClient udpClient;
        private static async Task<string> ReceiveMessageAsync()
        {
            string message = "";

            try
            {
                UdpReceiveResult result = await udpClient.ReceiveAsync();
                message = Encoding.Unicode.GetString(result.Buffer);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return message;
        }
        private static async Task SendMessageAsync(string msg)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(msg);
                await udpClient.SendAsync(data, data.Length, CLIENT_IP, CLIENT_PORT);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static async Task Main(string[] args)
        {
            try
            {
                udpClient = new UdpClient(SERVER_PORT);
                Console.WriteLine($"Server has been started on {SERVER_PORT} PORT");
                await Task.Run(() =>  MenuAsync());
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static async Task MenuAsync()
        {
            Model model = new Model();
            View view = new View();

            await SendMessageAsync("connect");
            while (true)
            {
                string option = await ReceiveMessageAsync();
                switch (option)
                {
                    case "get_data":
                        {
                            try
                            {
                                await SendMessageAsync(view.GetData(model.People));

                            }
                            catch (Exception e) { Console.WriteLine(e.Message); }
                            break;
                        }
                    case "save_data":
                        {
                            string input = await ReceiveMessageAsync();
                            try
                            {
                                model.UpdateDataBase(input);
                            }
                            catch (Exception e) {Console.WriteLine(e.Message); }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}
