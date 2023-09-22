using System;
using System.Collections.Generic;
using System.Text;
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
        public string GetData(Human human)
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("{0,-20}|| {1,-20}|| {2,-5}|| {3}", "First name", "Last name", "Age", "Is alive").AppendLine();
            output.AppendFormat("{0,-20}|| {1,-20}|| {2,-5}|| {3}", human.First_name, human.Last_name, human.Age, human.IsAlive).AppendLine();
            return output.ToString();
        }
        public string EnterNewData()
        {
            Console.WriteLine("Enter data about new object in following way:\nFirst Name,Last Name,Age,Is this person alive(true or false)");
            return Console.ReadLine().Replace(',', ';');
        }

    }
}
