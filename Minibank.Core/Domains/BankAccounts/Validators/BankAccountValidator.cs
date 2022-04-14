using FluentValidation;
using Minibank.Core.Utility;

namespace Minibank.Core.Domains.BankAccounts.Validators
{
    public class BankAccountValidator : AbstractValidator<BankAccount>
    {
        public BankAccountValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage($"{ValidationMessages.EmptyField}");
            RuleFor(x => x.Currency).IsInEnum()
                .WithMessage($"{ValidationMessages.EmptyField}");
        }
    }
}
