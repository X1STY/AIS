using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using lab2_Server_AIS;
using System.Collections.ObjectModel;

namespace WPF_Client
{
    public class Model : BaseModel
    {
        static IniFile cfg = new IniFile(@"C:\Users\X1STY-\source\repos\X1STY\AIS\config.ini");

        private int CLIENT_PORT = Int32.Parse(cfg.Read("CLIENT_PORT", "AIS"));
        private int SERVER_PORT = Int32.Parse(cfg.Read("SERVER_PORT", "AIS"));
        private string SERVER_IP = cfg.Read("IP", "AIS");

        UdpClient udpClient;

        

        public Model()
        {
            udpClient = new UdpClient(CLIENT_PORT);
            RecieveMessage();
        }

        public ObservableCollection<User> GetDataFromServer()
        {
            var pers = new ObservableCollection<User>();
            SendMessage("get_data");
            string[] data = RecieveMessage().Split('\n');
            foreach (var item in data)
            {
                User person = new User().ToStruct(item);
                pers.Add(person);
            }
            return pers;
        }

        public void SaveData(ObservableCollection<User> persons)
        {
            string message = GetData(persons);
            SendMessage("save_data");
            SendMessage(message);
        }

        public string GetData(ObservableCollection<User> people)
        {
            string output = "";
            int count = people.Count;
            for (int i = 0; i < count; i++)
            {
                output += people[i].ToString();
                if (i < count - 1)
                {
                    output += '\n';
                }
            }
            return output;
        }

        private string RecieveMessage()
        {
            var remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIP);
                string message = Encoding.Unicode.GetString(data);
                return message;
            }
            catch (Exception e) { return e.Message; }
        }

        private  void SendMessage(string msg)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(msg);
                udpClient.Send(data, data.Length, SERVER_IP, SERVER_PORT);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }


       
    }
}
