using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Domain.Errors;
using PaymentGatewayApi.Contracts;

namespace PaymentGatewayApi.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentCommandHandler _paymentCommandHandler;
        private readonly IPaymentQueryHandler _paymentQueryHandler;

        public PaymentController(IPaymentCommandHandler paymentCommandHandler, IPaymentQueryHandler paymentQueryHandler)
        {
            _paymentCommandHandler = paymentCommandHandler;
            _paymentQueryHandler = paymentQueryHandler;
        }

        [HttpPost]
        [Route("merchant/{merchantId}/payment")]
        public async Task<IActionResult> Post(PaymentRequest request, string merchantId)
        {
            var result = await _paymentCommandHandler.Handle(request.ToPaymentCommand(merchantId));

            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value.ToPaymentResponse());
            }

            if (result.IsFailed && result.Reasons.OfType<ExpiredCardError>().Any())
            {
                return new BadRequestObjectResult(result.ValueOrDefault?.ToPaymentResponse("Invalid card"));
            }

            if (result.IsFailed && result.Reasons.OfType<MerchantNotFoundError>().Any())
            {
                return new BadRequestObjectResult(result.ValueOrDefault?.ToPaymentResponse("Merchant not found"));
            }

            return new ObjectResult(new ErrorResponse("Unexpected error"))
                {StatusCode = (int) HttpStatusCode.InternalServerError};
        }

        [HttpGet]
        [Route("merchant/{merchantId}/payment/{paymentId}")]
        public async Task<IActionResult> Get(string merchantId, string paymentId)
        {
            var result = await _paymentQueryHandler.Get(paymentId, merchantId);

            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value.ToPaymentResponse());
            }

            if (result.IsFailed && result.Reasons.OfType<PaymentNotFoundError>().Any())
            {
                return new NotFoundObjectResult(new ErrorResponse("Payment not found"));
            }

            return new ObjectResult(new ErrorResponse("Unexpected error"))
                { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}
