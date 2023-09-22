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
            Model model = new Model();
            View view = new View();

            while (true)
            {
                Console.WriteLine("1. Get all records\n2. Get a record by it's number\n3. Add a record\n4. Delete a record\nESC. Exit\n");
                ConsoleKey option = Console.ReadKey().Key;
            
                Console.WriteLine(option);
                switch (option)
                {
                    case ConsoleKey.D1:
                        {   
                            Console.Clear();
                            Console.WriteLine($"{view.GetData(model.People)}\n");
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            Console.Clear();
                            Console.WriteLine($"Enter number of record you need (1-{model.People.Count})");
                            Int32.TryParse(Console.ReadLine(), out int recordNumber);
                            try
                            {
                                Console.WriteLine(view.GetData(model.GetSingleRecord(recordNumber)));
                            } catch (Exception e) { Console.WriteLine(e.Message); }
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            Console.Clear();
                            string input = view.EnterNewData();
                            try
                            {
                                Console.WriteLine(model.AddRecord(model.People, input));
                            } catch (Exception e) { Console.WriteLine(e.Message); } 
                            break;
                        }
                    case ConsoleKey.D4:
                        {
                            Console.Clear();
                            Console.WriteLine($"Enter number of record to delete (1-{model.People.Count})\n");
                            Int32.TryParse(Console.ReadLine(), out int recordNumber);
                            try
                            {
                                Console.WriteLine(model.DeleteRecord(model.People, recordNumber));
                            } catch (Exception e) { Console.WriteLine(e.Message); }
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default: 
                        {
                            Console.Clear();
                            Console.WriteLine($"Incorrect input {option}\n");
                            break;
                        }
                }
            }
        }
    }
}
