using System;
using FluentAssertions;
using PaymentGateway.Domain;
using Xunit;

namespace PaymentGateway.Tests.UnitTests.Domain
{
    public class CardTests
    {
        [Fact]
        public void IsExpired_ReturnsTrue_When_ExpiryMonth_And_Year_AreInThePast()
        {
            var card = new Card("", "", DateTime.Now.Month - 1, DateTime.Now.Year, "");

            card.IsExpired(DateTime.UtcNow).Should().BeTrue();
        }

        [Fact]
        public void IsExpired_ReturnsFalse_When_ExpiryMonth_And_Year_AreCurrentMonth()
        {
            var card = new Card("", "", DateTime.Now.Month, DateTime.Now.Year, "");

            card.IsExpired(DateTime.UtcNow).Should().BeFalse();
        }

        [Fact]
        public void IsExpired_ReturnsFalse_When_ExpiryMonth_And_Year_AreInTheFuture()
        {
            var card = new Card("", "", DateTime.Now.Month +1, DateTime.Now.Year, "");

            card.IsExpired(DateTime.UtcNow).Should().BeFalse();
        }
    }
}
