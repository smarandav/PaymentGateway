using System.Threading.Tasks;
using FluentResults;

namespace PaymentGateway.Application.PaymentApis
{
    public interface IAcquiringBank
    {
        Task<Result<PaymentResponse>> RequestPayment(PaymentRequest paymentRequest);
    }
}