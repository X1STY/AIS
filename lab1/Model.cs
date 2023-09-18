using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace lab1
{
    public class Model
    {
        const string pathToCSV = @"C:\Users\X1STY-\Desktop\KAVO.csv";
        private List<Human> people;
        public List<Human> People { get { return people; } }
        public struct Human
        {
            private string first_name;
            private string last_name;
            private int age;
            private bool isAlive;

            public Human() {
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
            public string First_name { get { return first_name; } set { this.first_name = value; } }
            public string Last_name { get { return last_name; } set { this.last_name = value; } }
            public int Age { get { return age; } set { this.age = value; } }
            public bool IsAlive { get { return isAlive; } set { this.isAlive = value; } }
        }

        
        public Model()
        {

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
                    foreach (string line in lines)
                    {
                        string[] fields = line.Split(';');
                        if (fields.Length == 4) people.Add(new Human(fields[0], fields[1], Int32.Parse(fields[2]), bool.Parse(fields[3])));
                        else break;
                    }
                    sr.Close();
                }
            }
            catch (Exception e) { Console.WriteLine($"Caught an Exception:\n{e}"); }
        }

        public void updateCSV(List<Human> people, bool addition)
        {
            using (StreamWriter sw = new StreamWriter(pathToCSV, addition))
            {
                foreach (Human human in people)
                {
                    sw.WriteLine($"{human.First_name};{human.Last_name};{human.Age};{human.IsAlive}");
                }
                sw.Close();
            }
        }

        public string DeleteRecord(List<Human> people, int recordNumber) 
        {
            if (recordNumber <= 0 || recordNumber > people.Count) return $"There is no record with id {recordNumber}\n";
            people.Remove(people[recordNumber - 1]);
            updateCSV(people, false);
            return $"record №{recordNumber} deleted successfully";

        }
        public string AddRecord(List<Human> people, string[] input)
        {
            string _firstName = input[0];
            string _lastName = input[1];
            int _age = Int32.Parse(input[2]);
            bool _isAlive = Boolean.Parse(input[3]);
            people.Add(new Human(_firstName, _lastName,_age,_isAlive));
            updateCSV(people, false);
            return "New record added successfully";
        }
    }
}
