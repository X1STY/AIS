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
            var remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
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

            while (true)
            {
                await SendMessageAsync("1. Get all records\n2. Get a record by its number\n3. Add a record\n4. Delete a record\nESC. Exit\n");
                string option = await ReceiveMessageAsync();
                switch (option)
                {
                    case "1":
                        {
                            await SendMessageAsync($"{view.GetData(model.People)}\n");
                            break;
                        }
                    case "2":
                        {
                            await SendMessageAsync($"Enter number of record you need (1-{model.People.Count})");
                            Int32.TryParse(await ReceiveMessageAsync(), out int recordNumber);
                            try
                            {
                                await SendMessageAsync(view.GetData(model.GetSingleRecord(recordNumber)));
                            }
                            catch (Exception e) { await SendMessageAsync(e.Message); }
                            break;
                        }
                    case "3":
                        {
                            await SendMessageAsync("Enter data about a new object in the following way:\nFirst Name,Last Name,Age,Is this person alive(true or false)");
                            string input = await ReceiveMessageAsync();
                            try
                            {
                                await SendMessageAsync(model.AddRecord(model.People, input.Replace(',', ';')));
                            }
                            catch (Exception e) { await SendMessageAsync(e.Message); }
                            break;
                        }
                    case "4":
                        {
                            await SendMessageAsync($"Enter the number of the record to delete (1-{model.People.Count})\n");
                            Int32.TryParse(await ReceiveMessageAsync(), out int recordNumber);
                            try
                            {
                                await SendMessageAsync(model.DeleteRecord(model.People, recordNumber));
                            }
                            catch (Exception e) { await SendMessageAsync(e.Message); }
                            break;
                        }
                    default:
                        {
                            await SendMessageAsync($"Incorrect input {option}\n");
                            break;
                        }
                }
            }
        }
    }
}
