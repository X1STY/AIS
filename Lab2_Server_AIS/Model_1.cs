namespace lab2_Server_AIS
{
    using System;
    
    public partial class User
    {
        public System.Guid id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int age { get; set; }
        public bool is_alive { get; set; }

        public override string ToString()
        {
            return $"{first_name};{last_name};{age};{is_alive}";
        }

        public User ToStruct(string humanInString)
        {
            User newPerson = new User();
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
            newPerson.id = System.Guid.NewGuid();
            return newPerson;
        }
    }
}
