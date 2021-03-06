using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.Kata.Domain.Models;

namespace Checkout.Kata.Infrastructure.Services
{
    public class CheckoutSystem : ICheckOutSystem
    {
        private readonly IList<Product> _products;
        private readonly IList<Discount> _discounts;

        private char[] scannedProducts;

        public CheckoutSystem(IList<Product> products, IList<Discount> discounts)
        {
            _products = products;
            _discounts = discounts;
            scannedProducts = new char[] { };
        }

        public int GetTotalPrice()
        {
            int total = 0;
            int totalDiscount = 0;
            foreach (var scan in scannedProducts)
            {
                total += _products.Single(s => s.SKU == scan).Price;
            }

            totalDiscount = _discounts.Sum(discount => CalculateDiscount(discount, scannedProducts));
            return total - totalDiscount;
        }

        public void Scan(string item)
        {
            if (!String.IsNullOrEmpty(item))
            {
                scannedProducts = item
                    .ToCharArray()
                    .Where(scannedSKU => _products.Any(product => product.SKU == scannedSKU))
                    .ToArray();
            }
            else
            {
                scannedProducts = new char[] { };
            }
        }

        private int CalculateDiscount(Discount discount, char[] cart)
        {
            int itemCount = cart.Count(item => item == discount.SKU);
            return (itemCount / discount.Quantity) * discount.Value;
        }
    }
}