using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static lab1.Model;

namespace lab1
{
    public class View
    {
        public View() { }
        public string GetData(List<Human> people)
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("{0,-3}|| {1,-20}|| {2,-20}|| {3,-5}|| {4}", "ID", "First name", "Last name", "Age", "Is alive").AppendLine();
            int index = 1;
            foreach (Human human in people)
            {
                output.AppendFormat("{0,-3}|| {1,-20}|| {2,-20}|| {3,-5}|| {4}", index, human.First_name, human.Last_name, human.Age, human.IsAlive);
                output.AppendLine();
                index++;
            }
            return output.ToString();
        }
        public string GetData(List<Human> people, int recordNumber)
        {
            if (recordNumber <= 0 || recordNumber > people.Count) return $"There is no record with id {recordNumber}\n";
            StringBuilder output = new StringBuilder();
            output.AppendFormat("{0,-20}|| {1,-20}|| {2,-5}|| {3}", "First name", "Last name", "Age", "Is alive").AppendLine();
            output.AppendFormat("{0,-20}|| {1,-20}|| {2,-5}|| {3}", people[recordNumber - 1].First_name, people[recordNumber - 1].Last_name, people[recordNumber - 1].Age, people[recordNumber - 1].IsAlive).AppendLine();
            return output.ToString();
        }
        public string EnterNewData()
        {
            Console.WriteLine("Enter data about new object in following way:\nFirst Name,Last Name,Age,Is this person alive");
            return Console.ReadLine();
        }

    }
}
