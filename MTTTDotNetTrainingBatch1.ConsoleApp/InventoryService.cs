using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MTTTDotNetTrainingBatch1.ConsoleApp
{
    internal class InventoryService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "DoNetTrainingBatch1",
            UserID = "sa",
            Password = "sa@123",
            TrustServerCertificate = true,
        };

        public void CreateProduct()
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

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
            string query1 = "select * from Tbl_InventoryService1";
            SqlCommand cmd1 = new SqlCommand(query1, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();

            adapter.Fill(dt);



            string getMaxIdQuery = "SELECT ISNULL (MAX(Id), 0) + 1 FROM Tbl_InventoryService1"; //codes generated with gpt
            SqlCommand getMaxIdCmd = new SqlCommand(getMaxIdQuery, connection);
            int nextId = (int)getMaxIdCmd.ExecuteScalar();
            string productCode = "P" + nextId.ToString().PadLeft(3, '0');

            Product product = new Product(productCode, insertProductName, price, quantity, "Fruit");


            string query = @"INSERT INTO [dbo].[Tbl_InventoryService1]
           ([Code]
           ,[Name]
           ,[Price]
           ,[Quantity]
           ,[Category])
     VALUES
           (@Code
           ,@Name
           ,@Price
           ,@Quantity
           ,@Category)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Code", productCode);
            cmd.Parameters.AddWithValue("@Name", insertProductName);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@Category", "Fruit");

            cmd.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Product is successfully added.");
        }
        public void ViewProduct()
        {
            Console.WriteLine("Product List:");

            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from Tbl_InventoryService1";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            connection.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine("ID: " + dr["Id"]);
                Console.WriteLine("CODE: " + dr["Code"]);
                Console.WriteLine("NAME: " + dr["Name"]);
                Console.WriteLine("PRICE: " + dr["Price"]);
                Console.WriteLine("QUANTITY: " + dr["Quantity"]);
                Console.WriteLine("CATEGORY: " + dr["Category"]);
                Console.WriteLine("----------------------");
            }


        }

        public void UpdateProduct()
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

        UpdateProduct:

            Console.Write("Enter the Product Code: ");
            string UpdateProduct = Console.ReadLine()!;

            string query = $"select * from Tbl_InventoryService1 where Code  = '{UpdateProduct}'";

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            int rowCount = dt.Rows.Count;

            //var original = Data.Products.FirstOrDefault(x => x.Code == UpdateProduct);

            if (rowCount == 0)
            {
                Console.WriteLine("The code you enter is not in the product list!");
                goto UpdateProduct;
            }
            Console.WriteLine("Product Found!");

            DataRow dr = dt.Rows[0];

            Console.WriteLine("ID: " + dr["Id"]);
            Console.WriteLine("CODE: " + dr["Code"]);
            Console.WriteLine("NAME: " + dr["Name"]);
            Console.WriteLine("PRICE: " + dr["Price"]);
            Console.WriteLine("QUANTITY: " + dr["Quantity"]);
            Console.WriteLine("CATEGORY: " + dr["Category"]);
            Console.WriteLine("----------------------");


        //var product = original.Clone();
        BeforeQuantityUpdate:
            Console.Write("How many do you want to remove?: ");
            string quantity = Console.ReadLine()!;
            bool IsInt = int.TryParse(quantity, out int updateQuantity);
            if (!IsInt)
            {
                Console.WriteLine("Invalid Quantity");
                goto BeforeQuantityUpdate;
            }
            int value = (int)dt.Rows[0]["Quantity"];
            value -= updateQuantity;

            string query1 = $@"UPDATE [dbo].[Tbl_InventoryService1]
   SET [Quantity] = @Quantity
 WHERE [Code] = '{UpdateProduct}'";

            SqlCommand cmd1 = new SqlCommand(query1, connection);
            cmd1.Parameters.AddWithValue("@Quantity", value);

            cmd1.ExecuteNonQuery();

            connection.Close();

            Console.WriteLine("Product Updatated Successfully!");
        }
        public void DeleteProduct()
        {

            SqlConnection connection =  new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

        DeleteProduct:
            Console.Write("Enter the product code of the product that you want to delete: ");
            string deleteProduct = Console.ReadLine()!;
            string query = $"select * from Tbl_InventoryService1 where Code  = '{deleteProduct}'";

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            int rowCount = dt.Rows.Count;

            if (rowCount == 0)
            {
                Console.WriteLine("Product not found");
                goto DeleteProduct;
            }
            Console.WriteLine("Product Found!");

            DataRow dr = dt.Rows[0];

            Console.WriteLine("ID: " + dr["Id"]);
            Console.WriteLine("CODE: " + dr["Code"]);
            Console.WriteLine("NAME: " + dr["Name"]);
            Console.WriteLine("PRICE: " + dr["Price"]);
            Console.WriteLine("QUANTITY: " + dr["Quantity"]);
            Console.WriteLine("CATEGORY: " + dr["Category"]);
            Console.WriteLine("----------------------");

        BeforeAreYouSure:
            Console.Write("Are you sure you want to delete this row?(Y for yes, N for no): ");
            string choose = Console.ReadLine()!;

            if (choose.ToUpper() == "Y")
            {
                string query1 = $"delete from Tbl_InventoryService1 where Code = '{deleteProduct}'";

                SqlCommand cmd1 = new SqlCommand(query1, connection);
                cmd1.Parameters.AddWithValue("@Id", dr["Id"]);
                cmd1.Parameters.AddWithValue("@Code", dr["Code"]);
                cmd1.Parameters.AddWithValue("@Name", dr["Name"]);
                cmd1.Parameters.AddWithValue("@Price", dr["Price"]);
                cmd1.Parameters.AddWithValue("@Quantity", dr["Quantity"]);
                cmd1.Parameters.AddWithValue("@Category", "Fruit");

                cmd1.ExecuteNonQuery();

                Console.WriteLine("Product deleted successfully!");
            }
            else if (choose.ToUpper() == "N")
            {
                Console.WriteLine("Product Not Deleted!");
                return;
            }
            else
            {
                Console.WriteLine("Invalid Input!");
                goto BeforeAreYouSure;
            }

            connection.Close();

        }
    }
}
