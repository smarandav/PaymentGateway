using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Domain;

namespace PaymentGateway.Application.Commands
{
    public interface IPaymentCommandHandler
    {
        Task<Result<Payment>> Handle(PaymentCommand paymentCommand);
    }
}