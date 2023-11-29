using Lab6;

namespace Lab6AIS
{
    internal class Program
    {
        static void Main()
        {
            var listOfProducts = Parser.Parse(@"https://2droida.ru/collection/smart-chasy-i-fitnes-braslety");
            db.SaveProductsToDatabase(listOfProducts);
        }
    }
}
