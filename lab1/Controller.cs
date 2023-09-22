using System;

namespace lab1
{
    public class Controller
    {
        static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            int option=0;
            Model model = new Model();
            View view = new View();

            while (true)
            {
                Console.WriteLine("1. Get all records\n2. Get a record by it's number\n3. Add a record\n4. Delete a record\n5. Exit\n");
                bool check = Int32.TryParse(Console.ReadLine(), out option);
                if (!check) {Console.WriteLine("Incorrect input\n"); continue;}
            
                Console.WriteLine(option);
                switch (option)
                {
                    case 1:
                        {
                            Console.Clear();
                            Console.WriteLine($"{view.GetData(model.People)}\n");
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            Console.WriteLine($"Enter number of record you need (1-{model.People.Count})");
                            int recordNumber = Int32.Parse(Console.ReadLine());
                            try
                            {
                                Console.WriteLine(view.GetData(model.GetSingleRecord(recordNumber)));
                            } catch (Exception e) { Console.WriteLine(e.Message); }
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            string input = view.EnterNewData();
                            try
                            {
                                Console.WriteLine(model.AddRecord(model.People, input));
                            } catch (Exception e) { Console.WriteLine(e.Message); } 
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            Console.WriteLine($"Enter number of record to delete (1-{model.People.Count})\n");
                            int recordNumber = Int32.Parse(Console.ReadLine());
                            try
                            {
                                Console.WriteLine(model.DeleteRecord(model.People, recordNumber));
                            } catch (Exception e) { Console.WriteLine(e.Message); }
                            break;
                        }
                    case 5:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default: 
                        {
                            Console.Clear();
                            Console.WriteLine($"There is no option with number {option}\n");
                            break;
                        }
                }
            }
        }
    }
}
