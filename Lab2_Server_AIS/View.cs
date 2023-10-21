using System.Collections.Generic;
namespace lab2_Server_AIS
{
    public class View
    {
        public View() { }
        public string GetData(List<User> people)
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
    }
}
