using lab2_Server_AIS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client
{
    public  class Person: BaseModel
    {
        private string first_name { get; set; }
        private string last_name { get; set; }
        private int age { get; set; }
        private bool is_alive { get; set; }

        public string FirstName { get => first_name; set { first_name = value; OnPropertyChanged(nameof(FirstName)); } }
        public string LastName { get => last_name; set { last_name = value; OnPropertyChanged(nameof(LastName)); } }
        public int Age { get => age; set { age = value; OnPropertyChanged(nameof(Age)); } }
        public bool IsAlive { get => is_alive; set { is_alive = value; OnPropertyChanged(nameof(IsAlive)); } }
        public override string ToString()
        {
            return $"{first_name};{last_name};{age};{is_alive}";
        }
        public Person ToStruct(string humanInString)
        {
            Person newPerson = new Person();
            string[] data = humanInString.Split(';');
            if (data.Length != 4) { throw new Exception("Incorrect data type"); ; }
            newPerson.first_name = data[0].Trim();
            newPerson.last_name = data[1].Trim();

            bool ageCheck = Int32.TryParse(data[2], out int age);
            if (ageCheck) newPerson.age = age;
            else { throw new Exception("Incorrect age type\n"); }

            bool lifeStatusCheck = Boolean.TryParse(data[3], out bool isAlive);
            if (lifeStatusCheck) newPerson.is_alive = isAlive;
            else { throw new Exception("Incorrect life status type\n"); }
            return newPerson;
        }
    }
}
