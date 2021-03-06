using System;
using Checkout.Kata.Domain.Models;
using Checkout.Kata.Infrastructure.Services;
using Xunit;

namespace Checkout.Kata.Tests.Unit
{
    public class CheckoutTests
    {
        private readonly ICheckOutSystem _checkoutSystem;

        public CheckoutTests()
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
        }


        [Fact]
        public void When_No_Items_Scanned_Return_Zero()
        {
            var expectedPrice = 0;
            _checkoutSystem.Scan("");
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }

        [Theory]
        [InlineData("A", 50)]
        [InlineData("B", 30)]
        [InlineData("C", 20)]
        [InlineData("D", 15)]
        public void When_Single_Item_Scanned_Return_Correct_Price(string skuCode, int skuPrice)
        {
            var expectedPrice = skuPrice;
            _checkoutSystem.Scan(skuCode);
            var actualPrice = _checkoutSystem.GetTotalPrice();
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Theory]
        [InlineData("AA", 100)]
        [InlineData("AB", 80)]
        [InlineData("AC", 70)]
        [InlineData("AD", 65)]
        [InlineData("ABC", 100)]
        [InlineData("ABCD", 115)]
        [InlineData("ABCCDD", 150)]
        [InlineData("CDBA", 115)]
        public void When_Items_Without_Discount_Scanned_Return_Correct_Price(string skuCode, int skuPrice)
        {
            var expectedPrice = skuPrice;
            _checkoutSystem.Scan(skuCode);
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }


        [Theory]
        [InlineData("AAA", 130)]
        [InlineData("AAAB", 160)]
        [InlineData("AAABB", 175)]
        [InlineData("AAAAAA", 260)]
        [InlineData("BB", 45)]
        [InlineData("ABB", 95)]
        [InlineData("BBBB", 90)]
        [InlineData("BBBBACD", 175)]
        [InlineData("AAABBAAA", 305)]
        public void When_Items_With_Discount_Scanned_Returns_Discounted_Price(string skuCode, int skuPrice)
        {
            var expectedPrice = skuPrice;
            _checkoutSystem.Scan(skuCode);
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }


        [Theory]
        [InlineData("EEGHKYLP", 0)]
        [InlineData("ABCDE", 115)]
        [InlineData("AAAAZ", 180)]
        public void When_Invalid_Items_Scanned_Filter_Them(string skuCode, int skuPrice)
        {
            var expectedPrice = skuPrice;
            _checkoutSystem.Scan(skuCode);
            var actualPrice = _checkoutSystem.GetTotalPrice();

            Assert.Equal(expectedPrice, actualPrice);
        }

    }

}