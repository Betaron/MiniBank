﻿namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        /// <summary>
        /// Searches for a bank account by his id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <returns>Found Bank account</returns>
        Task<BankAccount> GetByIdAsync(string id);

        /// <summary>
        /// Searches for a bank account by user id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="userId">User identification number</param>
        /// <returns>Found bank accounts</returns>
        Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId);

        ///<summary>
        ///<br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>All bank accounts</returns>
        Task<IEnumerable<BankAccount>> GetAllAsync();

        /// <summary>
        /// Adds a new bank account. Copies an argument
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="account">Template bank account</param>
        Task CreateAsync(BankAccount account);

        /// <summary>
        /// Searches for a bank account by id
        /// and changes based on the passed
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="account">Bank account to be changed</param>
        Task UpdateAsync(BankAccount account);

        /// <summary>
        /// Deletes a bank account by id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        Task DeleteAsync(string id);

        /// <summary>
        /// Makes an account inactive and sets a closing date
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        Task CloseAccountAsync(string id);

        /// <summary>
        /// Changes account balance
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="amount">New account balance</param>
        Task UpdateBalanceAsync(string id, double amount);
    }
}
