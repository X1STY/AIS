using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;


namespace lab2_Server_AIS
{
    public class Model
    {
        const string pathToCSV = "KAVO.csv";
        private List<Human> people;
        static Logger logger;
        public List<Human> People { get { return people; } }
        public struct Human
        {
            private string first_name;
            private string last_name;
            private int age;
            private bool isAlive;

            public Human()
            {
                first_name = string.Empty;
                last_name = string.Empty;
                age = 0;
                isAlive = false;
            }
            public Human(string _first_name, string _last_name, int _age, bool _isAlive)
            {
                first_name = _first_name;
                last_name = _last_name;
                age = _age;
                isAlive = _isAlive;
            }
            public string First_name { get => first_name; set => first_name = value; }
            public string Last_name { get => last_name; set => this.last_name = value;  }
            public int Age { get => age; set => this.age = value; }
            public bool IsAlive { get => isAlive; set => this.isAlive = value; }

            public override string ToString()
            {
                return $"{first_name};{last_name};{age};{isAlive}";
            }

            public Human ToStruct(string humanInString)
            {
                Human newPerson = new Human();
                string[] data = humanInString.Split(';');
                if (data.Length != 4) { logger.Info("Incorrect data type");  throw new Exception("Incorrect data type"); ; }
                newPerson.First_name = data[0].Trim();
                newPerson.Last_name = data[1].Trim();

                bool ageCheck = Int32.TryParse(data[2], out int age);
                if (ageCheck) newPerson.Age = age;
                else {logger.Info("Incorrect age input"); throw new Exception("Incorrect age type\n"); }

                bool lifeStatusCheck = Boolean.TryParse(data[3], out bool isAlive);
                if (lifeStatusCheck) newPerson.IsAlive = isAlive;
                else {logger.Info("Incorrct life status"); throw new Exception("Incorrect life status type\n"); } 
                return newPerson;
            }
        }

        
        public Model()
        {
            logger = LogManager.GetCurrentClassLogger();
            people = new List<Human>();
            ReadCSV();
        }

        public void ReadCSV()
        {
            try
            {
                using (StreamReader sr = new StreamReader(pathToCSV))
                {
                    string rowData = sr.ReadToEnd();
                    string[] lines = rowData.Split('\n');
                    for (int i = 0; i<lines.Length-1; i++)
                    {
                        people.Add(new Human().ToStruct(lines[i]));
                    }
                    sr.Close();
                }
            }
            catch (Exception e) {logger.Error("db file not found"); Console.WriteLine($"Cannot read the file or file contains incorrect data\n{e.Message}\n"); Environment.Exit(1); }
        }

        public void updateCSV(List<Human> people, bool addition)
        {
            using (StreamWriter sw = new StreamWriter(pathToCSV, addition))
            {
                foreach (Human human in people)
                {
                    sw.WriteLine(human.ToString());
                }
                sw.Close();
            }
        }

        public string DeleteRecord(List<Human> people, int recordNumber) 
        {
            if (recordNumber <= 0 || recordNumber > people.Count) {logger.Error("Incorrect record number id");  throw new Exception($"There is no record with id {recordNumber}\n");  }
            people.Remove(people[recordNumber - 1]);
            updateCSV(people, false);
            return $"record №{recordNumber} deleted successfully";

        }
        public string AddRecord(List<Human> people, string input)
        {
            Human human = new Human().ToStruct(input);
            people.Add(human);
            updateCSV(new List<Human> { human }, true);
            return "New record added successfully";
        }

        public Human GetSingleRecord(int recordNumber)
        {
            if (recordNumber <= 0 || recordNumber > people.Count) { logger.Error("Unexisted record id");throw new Exception($"There is no record with id {recordNumber}\n"); }
            return people[recordNumber - 1];

        }

    }
}
