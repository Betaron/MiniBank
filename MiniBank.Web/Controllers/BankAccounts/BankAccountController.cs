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
        public async Task<BankAccountDto> GetById(string id)
        {
            var model = await _bankAccountService.GetByIdAsync(id);

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
        public async Task<IEnumerable<BankAccountDto>> GetByUserId(string userId)
        {
            var models = await _bankAccountService.GetByUserIdAsync(userId);

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
        public async Task<IEnumerable<BankAccountDto>> GetAll()
        {
            var models = await _bankAccountService.GetAllAsync();

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
        public async Task Create(CreateBankAccountDto model)
        {
            await _bankAccountService.CreateAsync(new BankAccount
            {
                UserId = model.UserId,
                Currency = model.Currency
            });
        }

        /// <summary>
        /// Searches bank account and changes based on the passed
        /// </summary>
        /// <param name="model">Bank account to be changed</param>
        [HttpPut("{id}")]
        public async Task Update(string id, UpdateBankAccountDto model)
        {
            await _bankAccountService.UpdateAsync(new BankAccount
            {
                Id = id,
                UserId = model.UserId,
                Currency = model.Currency
            });
        }

        /// <summary>
        /// Deletes a bank account by id
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _bankAccountService.DeleteAsync(id);
        }

        /// <summary>
        /// Makes an account inactive and sets a closing date
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        [HttpPost("close/{id}")]
        public async Task CloseAccount(string id)
        {
            await _bankAccountService.CloseAccountAsync(id);
        }

        /// <summary>
        /// Changes account balance
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <param name="amount">New account balance</param>
        [HttpPatch("balance/{id}/{amount}")]
        public async Task UpdateBalance(string id, double amount)
        {
            await _bankAccountService.UpdateBalanceAsync(id, amount);
        }

        /// <summary>
        /// Calculates the commission amount when transferring between two accounts
        /// </summary>
        /// <returns>Commission amount</returns>
        [HttpGet("/commission/{fromAccountId}/{toAccountId}/{amount}")]
        public Task<double> CalculateCommission(double amount, string fromAccountId, string toAccountId)
        {
            return _bankAccountService.CalculateCommissionAsync(amount, fromAccountId, toAccountId);
        }

        /// <summary>
        /// Transferring funds between accounts
        /// </summary>
        [HttpGet("/transaction/{fromAccountId}/{toAccountId}/{amount}")]
        public async Task MoneyTransaction(double amount, string fromAccountId, string toAccountId)
        { 
            await _bankAccountService.MoneyTransactionAsync(amount, fromAccountId, toAccountId);
        }
    }
}
