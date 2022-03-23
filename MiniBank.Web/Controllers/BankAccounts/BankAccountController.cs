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
        public BankAccountDto GetById(string id)
        {
            var model = _bankAccountService.GetById(id);

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
        public IEnumerable<BankAccountDto> GetByUserId(string userId)
        {
            return _bankAccountService.GetByUserId(userId).Select(it => new BankAccountDto
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
        public IEnumerable<BankAccountDto> GetAll()
        {
            return _bankAccountService.GetAll().Select(it => new BankAccountDto
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
        public void Create(NewBankAccountDto model)
        {
            _bankAccountService.Create(new BankAccount
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
        public void Update(string id, NewBankAccountDto model)
        {
            _bankAccountService.Update(new BankAccount
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
        public void Delete(string id)
        {
            _bankAccountService.Delete(id);
        }

        /// <summary>
        /// Makes an account inactive and sets a closing date
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        [HttpPost("close/{id}")]
        public void CloseAccount(string id)
        {
            _bankAccountService.CloseAccount(id);
        }

        /// <summary>
        /// Changes account balance
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <param name="amount">New account balance</param>
        [HttpPatch("balance/{id}/{amount}")]
        public void UpdateBalance(string id, double amount)
        {
            _bankAccountService.UpdateBalance(id, amount);
        }

        /// <summary>
        /// Calculates the commission amount when transferring between two accounts
        /// </summary>
        /// <returns>Commission amount</returns>
        [HttpGet("/commission/{fromAccountId}/{toAccountId}/{amount}")]
        public double CalculateCommission(double amount, string fromAccountId, string toAccountId)
        {
            return _bankAccountService.CalculateCommission(amount, fromAccountId, toAccountId);
        }

        /// <summary>
        /// Transferring funds between accounts
        /// </summary>
        [HttpGet("/transaction/{fromAccountId}/{toAccountId}/{amount}")]
        public void MoneyTransaction(double amount, string fromAccountId, string toAccountId)
        { 
            _bankAccountService.MoneyTransaction(amount, fromAccountId, toAccountId);
        }
    }
}
