using Dot.Net.WebApi.Domain;
using FluentValidation;

namespace WebApi.Domain
{
    public class BidListValidator : AbstractValidator<BidList>
    {
        public BidListValidator()
        {
            //RuleFor(x => x.BidListId).NotNull().WithMessage("Please");
            RuleFor(x => x.Account).NotNull().NotEmpty().WithMessage("Please provide an Account Number");
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Please provide a Type"); ;
            RuleFor(x => x.BidQuantity).NotNull().NotEmpty().WithMessage("Please provide Quantity"); ;
            //RuleFor(x => x.BidQuantity).GetType().
        }
    }
}
