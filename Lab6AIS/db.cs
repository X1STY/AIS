using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6AIS
{
    public class db
    { 
        public static void SaveProductsToDatabase(List<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Список продуктов пуст.");
                Environment.Exit(0);
            }
            int amountOfSaved = 0;
            Console.WriteLine("Идет сохранение в базу данных...");
            try
            {
                using (var dbContext = new Model())
                {
                    dbContext.SmartWatch.AddRange(products);
                    amountOfSaved = dbContext.SaveChanges();
                }
                Console.WriteLine("Успешно сохранено " + amountOfSaved + " единиц товара");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка сохранения в базу даныых\n" + ex.Message);
                Environment.Exit(1);
            }

        }

    }
}
