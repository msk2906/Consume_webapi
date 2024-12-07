using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Consume_webapi;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string baseUrl = "https://productwebapi20241206160242.azurewebsites.net/"; 

            using HttpClient client = new HttpClient();
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Get All Products");
                Console.WriteLine("2. Get Product by ID");
                Console.WriteLine("3. Add a Product");
                Console.WriteLine("4. Update a Product");
                Console.WriteLine("5. Delete a Product");

                int choice = int.Parse(Console.ReadLine() ?? "0");

                switch (choice)
                {
                    case 1:
                        await GetAllProducts(client, baseUrl);
                        break;
                    case 2:
                        await GetProductById(client, baseUrl);
                        break;
                    case 3:
                        await AddProduct(client, baseUrl);
                        break;
                    case 4:
                        await UpdateProduct(client, baseUrl);
                        break;
                    case 5:
                        await DeleteProduct(client, baseUrl);
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
                Console.ReadKey();
            }
        }

        static async Task GetAllProducts(HttpClient client, string baseUrl)
        {
            var products = await client.GetFromJsonAsync<List<Product>>(baseUrl + "api/products/getall");
            Console.WriteLine("Products:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductID}: {product.ProductName}, Price: {product.Price}, Quantity: {product.Quantity}");
            }
        }

        static async Task GetProductById(HttpClient client, string baseUrl)
        {
            Console.Write("Enter Product ID: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            var response = await client.GetAsync($"{baseUrl}/api/products/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<Product>();
                Console.WriteLine($"Product: {product.ProductName}, Price: {product.Price}, Quantity: {product.Quantity}");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        static async Task AddProduct(HttpClient client, string baseUrl)
        {
            int productId = HelperModules.GetIntegerInput("Enter Product ID: ");
            string productName = HelperModules.GetStringInput("Enter Product Name: ");
            decimal price = HelperModules.GetDecimalInput("Enter Price: ");
            int quantity = HelperModules.GetIntegerInput("Enter Quantity: ");

            var newProduct = new Product
            {
                
                ProductID = productId,
                ProductName = productName,
                Price = price,
                Quantity = quantity
            };

            var response = await client.PostAsJsonAsync($"{baseUrl}/api/products/add", newProduct);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Product added successfully!");
            }
            else
            {
                Console.WriteLine("Error adding product.");
            }
        }

        static async Task UpdateProduct(HttpClient client, string baseUrl)
        {
            Console.Write("Enter Product ID to update: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            string productName = HelperModules.GetStringInput("Enter Product Name: ");
            decimal price = HelperModules.GetDecimalInput("Enter Price: ");
            int quantity = HelperModules.GetIntegerInput("Enter Quantity: ");

            var updatedProduct = new Product
            {
                ProductName = productName,
                Price = price,
                Quantity = quantity
            };

            var response = await client.PutAsJsonAsync($"{baseUrl}/api/products/update/{id}", updatedProduct);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Product updated successfully!");
            }
            else
            {
                Console.WriteLine("Error updating product.");
            }
        }

        static async Task DeleteProduct(HttpClient client, string baseUrl)
        {
            Console.Write("Enter Product ID to delete: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            var response = await client.DeleteAsync($"{baseUrl}/api/products/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Product deleted successfully!");
            }
            else
            {
                Console.WriteLine("Error deleting product.");
            }
        }
    }
}
