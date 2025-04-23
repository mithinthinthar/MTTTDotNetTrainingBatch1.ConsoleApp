using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTTTDotNetTrainingBatch1.ConsoleApp
{
    internal class InventoryService
    {
        public void CreateProduct()
        {
            Console.Write("Insert product name: ");
            string insertProductName = Console.ReadLine()!;

        InsertPriceAgain:
            Console.Write("Insert product price: ");
            string insertProductPrice = Console.ReadLine()!;
            bool isDecimal = decimal.TryParse(insertProductPrice, out decimal price);
            if (!isDecimal)
            {
                Console.WriteLine("Invalid Value!");
                goto InsertPriceAgain;
            }

        InsertQuantityAgain:
            Console.Write("Insert product quantity: ");
            string insertProductQuantity = Console.ReadLine()!;
            bool isInt = int.TryParse(insertProductQuantity, out int quantity);
            if (!isInt)
            {
                Console.WriteLine("Invalid Value!");
                goto InsertQuantityAgain;
            }


            Data.ProductId++;
            string productCode = "P" + Data.ProductId.ToString().PadLeft(3, '0');
            Product product = new Product(Data.ProductId, productCode, insertProductName, price, quantity, "Fruit");
            Data.Products.Add(product);

            Console.WriteLine("Product is successfully added.");
        }
        public void ViewProduct()
        {
            Console.WriteLine("Product List:");
            foreach (var product in Data.Products)
            {
                Console.WriteLine($"ID: {product.Id}, CODE: {product.Code}, NAME: {product.Name}, PRICE: {product.Price}, QUANTITY: {product.Quantity}, CATEGORY: Fruit");
            }
        }

        public void UpdateProduct()
        {

        UpdateProduct:

            Console.Write("Enter the Product Code: ");
            string UpdateProduct = Console.ReadLine()!;

            var original = Data.Products.FirstOrDefault(x => x.Code == UpdateProduct);

            if (original is null)
            {
                Console.WriteLine("The code you enter is not in the product list!");
                goto UpdateProduct;
            }
            Console.WriteLine("Product Found!");
            Console.WriteLine($"id: {original.Id}, code: {original.Code}, name: {original.Name}, price: {original.Price}, quantity: {original.Quantity}, Category: Fruit");

            var product = original.Clone();
        BeforeQuantityUpdate:
            Console.Write("How many do you want to remove?: ");
            string quantity = Console.ReadLine()!;
            bool IsInt = int.TryParse(quantity, out int updateQuantity);
            if (!IsInt)
            {
                Console.WriteLine("Invalid Quantity");
                goto BeforeQuantityUpdate;
            }
            product.Quantity -= updateQuantity;




            Console.WriteLine("Product Updatated Successfully!");
        }
        public void DeleteProduct()
        {
        DeleteProduct:
            Console.Write("Enter the product code of the product that you want to delete: ");
            string deleteProduct = Console.ReadLine()!;
            var product = Data.Products.FirstOrDefault(x => x.Code == deleteProduct);

            if (product is null)
            {
                Console.WriteLine("Product no found");
                goto DeleteProduct;
            }
            Console.WriteLine("Product Found!");
            Console.WriteLine($"id: {product.Id}, code: {product.Code}, name: {product.Name}, price: {product.Price}, quantity: {product.Quantity}, Category: Fruit");


            Data.Products.Remove(product);
            Console.WriteLine("Product deleted successfully!");


        }
    }
}
