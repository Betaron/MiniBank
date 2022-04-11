using FluentValidation;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Utility;

namespace Minibank.Core.Domains.BankAccounts.Validators
{
    public class FindUserValidator : AbstractValidator<BankAccount>
    {
        public FindUserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.UserId).MustAsync(async (userId, cancellation) =>
                    await userRepository.ExistsAsync(userId, cancellation))
                .WithMessage("{PropertyValue}" + $" - {ValidationMessages.ObjectNotFound}");
        }
    }
}
