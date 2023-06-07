using Microsoft.AspNetCore.Mvc;

namespace PaymentGatewayApi.Contracts
{
    public class ErrorResponse
    {
        public ErrorResponse(string message, SerializableError error)
        {
            Message = message;
            Error = error;
        }

        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse()
        {
            
        }

        public string Message { get; set; }

        public SerializableError Error { get; set; }
    }
}
