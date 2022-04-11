using FluentValidation;
using Minibank.Core.Utility;

namespace Minibank.Core.Domains.Users.Validators
{
    public class EmptyFieldsValidator : AbstractValidator<User>
    {
        public EmptyFieldsValidator()
        {
            RuleFor(x => x.Login).NotEmpty()
                .WithMessage(ValidationMessages.EmptyField);
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage(ValidationMessages.EmptyField);
        }
    }
}
