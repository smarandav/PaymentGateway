using System;

namespace PaymentGateway.Application
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}