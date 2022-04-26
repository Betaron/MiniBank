using FluentValidation;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Utility;

namespace Minibank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Login).NotEmpty()
                .WithMessage(ValidationMessages.EmptyField);
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage(ValidationMessages.EmptyField);
            RuleFor(x => x.Login).MustAsync(async (login, cancellation) =>
                    !await userRepository.UserExistsByLoginAsync(login, cancellation))
                .WithMessage("{PropertyValue} - занят");
        }
    }
}
