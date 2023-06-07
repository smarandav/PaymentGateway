using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Errors;
using Xunit;

namespace PaymentGateway.Tests.UnitTests.Domain
{
    public class PaymentTests
    {
        [Fact]
        public void AddPaymentCard_ReturnsExpiredCardError_WhenCardIsExpired()
        {
            var payment = new Payment("", 123, "GBP");

            var result = payment.AddPaymentCard(new Card("", "", DateTime.UtcNow.Month -1, DateTime.UtcNow.Year, ""), DateTime.UtcNow);

            result.Errors.OfType<ExpiredCardError>().Any().Should().BeTrue();
        }

        [Fact]
        public void AddPaymentCard_SetsPaymentCard_WhenCardIsNotExpired()
        {
            var nameOnCard = "L S Smith";
            var cardNumber = "2345678";
            var payment = new Payment("", 123, "GBP");

            var result = payment.AddPaymentCard(new Card(cardNumber, nameOnCard, DateTime.UtcNow.Month + 1, DateTime.UtcNow.Year, ""), DateTime.UtcNow);

            result.IsSuccess.Should().BeTrue();
            payment.Card.NameOnCard.Should().Be(nameOnCard);
            payment.Card.Number.Should().Be(cardNumber);
        }

        [Fact]
        public void CompletePayment_SetsSuccessStatus_WhenCurrentStatusIsInProgress()
        {
            var payment = new Payment("", 123, "GBP");

            var result = payment.CompletePayment();

            result.IsSuccess.Should().BeTrue();
            payment.Status.Should().Be(PaymentStatus.Success);
        }

        [Theory]
        [InlineData(PaymentStatus.Failed)]
        [InlineData(PaymentStatus.Success)]
        public void CompletePayment_SetsSuccessStatus_WhenCurrentStatusIs(PaymentStatus currentStatus)
        {
            var payment = new Payment("", "", 123, "GBP", new Card("", "", 2, 2022, ""), currentStatus);

            var result = payment.CompletePayment();

            result.IsSuccess.Should().BeFalse();
            result.Errors.OfType<PaymentStatusTransitionError>().Any().Should().BeTrue();
        }
        
        [Fact]
        public void Test()
        {
            var result = List(1234);

            result.Should().BeEquivalentTo(new int [] { 1, 2, 3, 4});
        }

        private int Count(int money, int[] coins)
        {
            var coinsList = coins.ToList().OrderByDescending(i => i);

            return 1;
        }

        /**
         * Given a non-negative integer, return an array or a list 
         * of the individual digits in order
         */
        private IEnumerable<int> List(int number)
        {
            var result = new List<int>();

            var reminder = 1;
            while (reminder > 0)
            {
                reminder = number % 10;
                number /= 10;
                result.Add(reminder);
            }

            result.Reverse();

            return result;
        }
    }
}