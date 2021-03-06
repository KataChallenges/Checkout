using System;
using System.Collections.Generic;
using Xunit;

namespace Checkout.Kata.Tests.Unit
{
    public class CheckoutTests
    {
        private readonly ICheckOutSystem _checkoutSystem;

        public CheckoutTests()
        {
            IList<Product> itemsList = new[]
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

            _checkoutSystem = new CheckoutSystem(itemsList, discounts);
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
            var expectedPrice = 100;
            _checkoutSystem.Scan("A");
            _checkoutSystem.Scan("B");
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }


        [Fact]
        public void When_Items_With_Discount_Scanned_Returns_Discounted_Price()
        {
            var expectedPrice = 130;
            _checkoutSystem.Scan("A");
            _checkoutSystem.Scan("A");
            _checkoutSystem.Scan("A");
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }


        [Fact]
        public void When_Invalid_Items_Scanned_Filter_Them()
        {
            var expectedPrice = 130;
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

    internal class Discount
    {
        public char SKU { get; set; }
        public int Quantity { get; set; }
        public int Value { get; set; }
    }

    internal class Product
    {
        public char SKU { get; set; }
        public int Price { get; set; }
    }

    internal class CheckoutSystem : ICheckOutSystem
    {
        public CheckoutSystem(IEnumerable<Product> itemsList, IEnumerable<Discount> discounts)
        {
            throw new NotImplementedException();
        }

        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }

        public void Scan(string item)
        {
            throw new NotImplementedException();
        }
    }

}