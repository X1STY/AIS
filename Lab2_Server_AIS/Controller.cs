using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace lab2_Server_AIS
{
    public class Controller
    {
        static IniFile cfg = new IniFile(@"C:\Users\X1STY-\source\repos\X1STY\AIS\config.ini");

        private static int CLIENT_PORT = Int32.Parse(cfg.Read("CLIENT_PORT", "AIS"));
        private static int SERVER_PORT = Int32.Parse(cfg.Read("SERVER_PORT", "AIS"));
        private static string CLIENT_IP = cfg.Read("IP", "AIS");

        static UdpClient udpClient;
        private static string RecieveMessage()
        {
            var remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
            string message = "";

            try
            {
                byte[] data = udpClient.Receive(ref remoteIP);
                message = Encoding.Unicode.GetString(data);
                Console.WriteLine($"User send {message} message");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return message;
        }

        private static void SendMessage(string msg)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(msg);
                udpClient.Send(data, data.Length, CLIENT_IP, CLIENT_PORT);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static void Main(string[] args)
        {
            try
            {
                udpClient = new UdpClient(SERVER_PORT);
                Console.WriteLine($"Server has been started on {SERVER_PORT} PORT");
                Menu();
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }

            static void Menu()
            {
                Model model = new Model();
                View view = new View();

                while (true)
                {
                SendMessage("1. Get all records\n2. Get a record by it's number\n3. Add a record\n4. Delete a record\nESC. Exit\n");
                string option = RecieveMessage();
                Console.WriteLine(option);
                switch (option)
                {
                    case "1":
                        {
                            SendMessage($"{view.GetData(model.People)}\n");
                            break;
                        }
                    case "2":
                        {
                            SendMessage($"Enter number of record you need (1-{model.People.Count})");
                            Int32.TryParse(RecieveMessage(), out int recordNumber);
                            try
                            {
                                SendMessage(view.GetData(model.GetSingleRecord(recordNumber)));
                            }
                            catch (Exception e) { SendMessage(e.Message); }
                            break;
                        }
                    case "3":
                        {
                            SendMessage("Enter data about new object in following way:\nFirst Name,Last Name,Age,Is this person alive(true or false)");
                            string input = RecieveMessage().Replace(',', ';');
                            try
                            {
                                SendMessage(model.AddRecord(model.People, input));
                            }
                            catch (Exception e) { SendMessage(e.Message); }
                            break;
                        }
                    case "4":
                        {
                            SendMessage($"Enter number of record to delete (1-{model.People.Count})\n");
                            Int32.TryParse(RecieveMessage(), out int recordNumber);
                            try
                            {
                                SendMessage(model.DeleteRecord(model.People, recordNumber));
                            }
                            catch (Exception e) { SendMessage(e.Message); }
                            break;
                        }
                    default:
                        {
                            SendMessage($"Incorrect input {option}\n");
                            break;
                        }
                    }
                }
            }
        }
    }
