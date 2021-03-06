using System;
using Checkout.Kata.Domain.Models;
using Checkout.Kata.Infrastructure.Services;

namespace Checkout.Kata.App
{
    class Program
    {
        private static ICheckOutSystem _checkoutSystem;
        static void Main(string[] args)
        {
            var SkuList = new[]
            {
                new Product{SKU = 'A', Price = 50},
                new Product{SKU = 'B', Price = 30},
                new Product{SKU = 'C', Price = 20},
                new Product{SKU = 'D', Price = 15}
            };

            var discounts = new[]
            {
                new Discount{SKU = 'A', Quantity = 3, Value = 20},
                new Discount{SKU = 'B', Quantity = 2, Value = 15}
            };

            _checkoutSystem = new CheckoutSystem(SkuList, discounts);

            Console.WriteLine("Welcome to Checkout Kata");

            Console.WriteLine("Please enter below the products (e.g. A, B, C, D, or combination AAB, AAA, ABC etc.) you want to scan:");

            var products = Console.ReadLine();

            _checkoutSystem.Scan(products);
            var total = _checkoutSystem.GetTotalPrice();

            Console.WriteLine($"Total Price: {total}");


        }
    }
}
