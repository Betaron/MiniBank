using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Web.Controllers.BankAccounts.Dto;

namespace Minibank.Web.Controllers.BankAccounts
{
    [ApiController]
    [Route("bank-account")]
    public class BankAccountController
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        /// <summary>
        /// Searches for a bank account by his id
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <returns>Found Bank account</returns>
        [HttpGet("{id}")]
        public async Task<BankAccountDto> GetById(Guid id, CancellationToken cancellationToken)
        {
            var model = await _bankAccountService.GetByIdAsync(id, cancellationToken);

            return new BankAccountDto
            {
                Id = model.Id,
                UserId = model.UserId,
                AccountBalance = model.AccountBalance,
                Currency = model.Currency,
                IsActive = model.IsActive,
                OpeningDate = model.OpeningDate,
                ClosingDate = model.ClosingDate
            };
        }

        /// <summary>
        /// Searches for a bank account by user id
        /// </summary>
        /// <param name="userId">User identification number</param>
        /// <returns>Found bank accounts</returns>
        [HttpGet("user-id/{userId}")]
        public async Task<IEnumerable<BankAccountDto>> GetByUserId(
            Guid userId, CancellationToken cancellationToken)
        {
            var models = 
                await _bankAccountService.GetByUserIdAsync(userId, cancellationToken);

            return models.Select(it => new BankAccountDto
            {
                Id = it.Id,
                UserId = it.UserId,
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });
        }

        /// <summary>
        /// Find all bank accounts
        /// </summary>
        /// <returns>All bank accounts from repository</returns>
        [HttpGet]
        public async Task<IEnumerable<BankAccountDto>> GetAll(CancellationToken cancellationToken)
        {
            var models = 
                await _bankAccountService.GetAllAsync(cancellationToken);

            return models.Select(it => new BankAccountDto
            {
                Id = it.Id,
                UserId = it.UserId,
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });
        }

        /// <summary>
        /// Adds a new bank account to the repository. Copies an argument
        /// </summary>
        /// <param name="model">Template bank account</param>
        [HttpPost]
        public Task Create(CreateBankAccountDto model, CancellationToken cancellationToken)
        {
            return _bankAccountService.CreateAsync(new BankAccount
            {
                UserId = model.UserId,
                Currency = model.Currency
            }, cancellationToken);
        }

        /// <summary>
        /// Searches bank account and changes based on the passed
        /// </summary>
        /// <param name="model">Bank account to be changed</param>
        [HttpPut("{id}")]
        public  Task Update(
            Guid id, UpdateBankAccountDto model, CancellationToken cancellationToken)
        {
            return _bankAccountService.UpdateAsync(new BankAccount
            {
                Id = id,
                UserId = model.UserId,
                Currency = model.Currency
            }, cancellationToken);
        }

        /// <summary>
        /// Deletes a bank account by id
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        [HttpDelete("{id}")]
        public Task Delete(Guid id, CancellationToken cancellationToken)
        {
            return _bankAccountService.DeleteAsync(id, cancellationToken);
        }

        /// <summary>
        /// Makes an account inactive and sets a closing date
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        [HttpPost("close/{id}")]
        public Task CloseAccount(Guid id, CancellationToken cancellationToken)
        {
            return _bankAccountService.CloseAccountAsync(id, cancellationToken);
        }

        /// <summary>
        /// Changes account balance
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <param name="amount">New account balance</param>
        [HttpPatch("balance/{id}/{amount}")]
        public Task UpdateBalance(
            Guid id, double amount, CancellationToken cancellationToken)
        {
            return _bankAccountService.UpdateBalanceAsync(id, amount, cancellationToken);
        }

        /// <summary>
        /// Calculates the commission amount when transferring between two accounts
        /// </summary>
        /// <returns>Commission amount</returns>
        [HttpGet("/commission/{fromAccountId}/{toAccountId}/{amount}")]
        public Task<double> CalculateCommission(
            double amount,
            Guid fromAccountId,
            Guid toAccountId, 
            CancellationToken cancellationToken)
        {
            return _bankAccountService.CalculateCommissionAsync(
                amount, fromAccountId, toAccountId, cancellationToken);
        }

        /// <summary>
        /// Transferring funds between accounts
        /// </summary>
        [HttpGet("/transaction/{fromAccountId}/{toAccountId}/{amount}")]
        public Task MoneyTransaction(
            double amount,
            Guid fromAccountId,
            Guid toAccountId, 
            CancellationToken cancellationToken)
        {
            return _bankAccountService.MoneyTransactAsync(
                amount, fromAccountId, toAccountId, cancellationToken);
        }
    }
}
