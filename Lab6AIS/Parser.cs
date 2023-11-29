using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Lab6AIS;

namespace Lab6
{
    public class Parser
    {
        static string ExtractCharacteristic(HtmlDocument doc, string characteristicName)
        {
            var characteristicNode = doc.DocumentNode.SelectSingleNode($"//div[@class='product__property-name' and contains(text(), '{characteristicName}')]/following-sibling::div[@class='product__property-value']");
            return characteristicNode?.InnerText.Trim() ?? "";
        }

        static string GetProgressBar(int progress, int length = 20)
        {
            int progressBarLength = (int)Math.Ceiling((double)progress / 100 * length);
            progressBarLength = Math.Min(progressBarLength, length);
            string progressBar = new string('#', progressBarLength) + new string('-', length - progressBarLength);
            return progressBar;
        }

        public static List<Product> Parse(string baseUrl)
        {
            int currentPage = 1;
            List<Product> listOfProducts = new List<Product>();

            try
            {
                Console.CursorVisible = false;
                Console.WriteLine("Загрузка товаров...");

                HtmlWeb web = new HtmlWeb();

                while (true)
                {
                    string url = $"{baseUrl}?page={currentPage}";
                    HtmlDocument doc = web.Load(url);

                    string productXPath = "//div[@class='product-preview__content']";
                    var productNodes = doc.DocumentNode.SelectNodes(productXPath);

                    if (productNodes != null && productNodes.Count > 0)
                    {

                        int totalProducts = productNodes.Count;
                        int currentProduct = 0;
                        foreach (var productNode in productNodes)
                        {
                            string productName = productNode.SelectSingleNode(".//div[@class='product-preview__title']/a").InnerText.Trim();
                            string productLink = productNode.SelectSingleNode(".//div[@class='product-preview__title']/a").GetAttributeValue("href", "");

                            HtmlDocument productDetailsDoc = web.Load("https://2droida.ru" + productLink);

                            string brand = ExtractCharacteristic(productDetailsDoc, "Бренд");
                            string batteryCapacity = ExtractCharacteristic(productDetailsDoc, "Емкость аккумулятора");
                            string screenDiagonal = ExtractCharacteristic(productDetailsDoc, "Диагональ экрана");
                            string productPrice;
                            try
                            {
                                productPrice = productNode.SelectSingleNode(".//div[@class='product-preview__price']/span[@class='product-preview__price-cur']").InnerText.Trim();
                            }
                            catch { productPrice = "Не определена"; }

                            listOfProducts.Add(new Product
                            {
                                Name = productName,
                                Price = productPrice,
                                Link = productLink,
                                Brand = brand,
                                BatteryCapacity = batteryCapacity,
                                ScreenDiagonal = screenDiagonal
                            });

                            currentProduct++;

                            int progress = (int)((double)currentProduct / totalProducts * 100);

                            //Console.Write($"\r{String.Empty}");
                            Console.Write($"\rОбработка страницы {currentPage}, товар {currentProduct} из {totalProducts} - [{GetProgressBar(progress)}]                                        ");
                        }
                        currentPage++;
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine("\nОбработка завершена!");
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Невозможно разрешить удаленное имя: '2droida.ru'")) 
                { 
                    Console.WriteLine("Не удается установить соединение с 2droida");
                }
                 Console.WriteLine($"\nВозникла ошибка: {ex.Message}");
                Environment.Exit(1);
            }
            return listOfProducts;

        }
    }
}

