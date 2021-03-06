using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Checkout.Kata.Tests.Unit
{
    public class CheckoutTests
    {
        private readonly ICheckOutSystem _checkoutSystem;

        public CheckoutTests()
        {
            IList<Product> SkuList = new[]
            {
                new Product{SKU = 'A', Price = 50},
                new Product{SKU = 'B', Price = 30},
                new Product{SKU = 'C', Price = 20},
                new Product{SKU = 'D', Price = 15}
            };

            IList<Discount> discounts = new[]
            {
                new Discount{SKU = 'A', Quantity = 3, Value = 20},
                new Discount{SKU = 'B', Quantity = 2, Value = 15}
            };

            _checkoutSystem = new CheckoutSystem(SkuList, discounts);
        }


        [Fact]
        public void When_No_Items_Scanned_Return_Zero()
        {
            var expectedPrice = 0;
            _checkoutSystem.Scan("");
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void When_Single_Item_Scanned_Return_Correct_Price()
        {
            var expectedPrice = 50;
            _checkoutSystem.Scan("A");
            var actualPrice = _checkoutSystem.GetTotalPrice();
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void When_Items_Without_Discount_Scanned_Return_Correct_Price()
        {
            var expectedPrice = 80;
            _checkoutSystem.Scan("AB");
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }


        [Fact]
        public void When_Items_With_Discount_Scanned_Returns_Discounted_Price()
        {
            var expectedPrice = 130;
            _checkoutSystem.Scan("AAA");
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void When_Invalid_Items_Scanned_Filter_Them()
        {
            var expectedPrice = 0;
            _checkoutSystem.Scan("Z");
            _checkoutSystem.Scan("E");
            _checkoutSystem.Scan("E");
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }

    }

    internal interface ICheckOutSystem
    {
        int GetTotalPrice();
        void Scan(string s);
    }

    public class Discount
    {
        public char SKU { get; set; }
        public int Quantity { get; set; }
        public int Value { get; set; }
    }

    public class Product
    {
        public char SKU { get; set; }
        public int Price { get; set; }
    }

    public class CheckoutSystem : ICheckOutSystem
    {
        private readonly IList<Product> _products;
        private readonly IList<Discount> _discounts;

        private char[] scannedProducts;
        public char[] ScannedProducts { get { return scannedProducts; } }

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

            foreach (var discount in _discounts)
            {
                int itemCount = scannedProducts.Count(item => item == discount.SKU);
                var singleDiscount = (itemCount / discount.Quantity) * discount.Value;
                totalDiscount += singleDiscount;
            }

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
    }

}