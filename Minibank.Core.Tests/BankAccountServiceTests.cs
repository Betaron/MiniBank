using System;
using Minibank.Core.Domains.BankAccounts.Services;
using Xunit;
using Moq;

namespace Minibank.Core.Tests
{
    public class BankAccountServiceTests
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountServiceTests(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }
    }
}