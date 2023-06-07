using FluentValidation;
using PaymentGatewayApi.Contracts;

namespace PaymentGatewayApi.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(x => x.CardExpiryMonth)
                .InclusiveBetween(1,12).WithMessage("CardExpiryMonth must be a valid month");
            RuleFor(x => x.CardExpiryYear)
                .InclusiveBetween(0,9999).WithMessage("CardExpiryYear must be between 0 and 9999");

            RuleFor(x => x.CardNumber)
                .NotNull().WithMessage("CardNumber must have a value")
                .Matches(@"^4[0-9]{12}(?:[0-9]{3})?$").WithMessage("CardNumber must have a value"); 
            //todo add support for other cards than visa or have a custom credit card validator

            RuleFor(x => x.Currency)
                .NotNull().WithMessage("Currency must have a value")
                .Matches(@"^[a-zA-Z]{3}").WithMessage("Currency not supported"); //todo use a currency library for validations

            RuleFor(x => x.Cvv)
                .NotNull().WithMessage("Cvv must have a value")
                .Matches(@"^[1-9]{3}").WithMessage("Cvv is invalid");

            RuleFor(x => x.NameOnCard)
                .NotNull().WithMessage("NameOnCard must have a value")
                .Matches(@"^((?:[A-Za-z]+ ?){1,3})$").WithMessage("NameOnCard is invalid");
        }
    }
}