using FluentValidation;

namespace Minibank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Login).NotEmpty().WithMessage(ValidationMessages.EmptyField);
            RuleFor(x => x.Email).NotEmpty().WithMessage(ValidationMessages.EmptyField);
        }
    }
}
