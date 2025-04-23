using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTTTDotNetTrainingBatch1.ConsoleApp
{
    internal class Data
    {
        public static int ProductId { get; set; } = 2;
        public static List<Product> Products = new List<Product>()
        {
            new Product (1, "P001", "Ko KO", 1m, 1, "Fruit"),
            new Product (2, "P002", "Ko KO2", 1.5m, 1, "Fruit")
        };
        
    }
}
