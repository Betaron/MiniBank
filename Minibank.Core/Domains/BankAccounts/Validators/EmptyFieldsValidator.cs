using FluentValidation;
using Minibank.Core.Utility;

namespace Minibank.Core.Domains.BankAccounts.Validators
{
    public class EmptyFieldsValidator : AbstractValidator<BankAccount>
    {
        public EmptyFieldsValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage($"{ValidationMessages.EmptyField}");
            RuleFor(x => x.Currency).IsInEnum()
                .WithMessage($"{ValidationMessages.EmptyField}");
        }
    }
}
