﻿namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        /// <summary>
        /// Searches for a bank account by his id
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <returns>Found Bank account</returns>
        BankAccount GetById(string id);

        /// <summary>
        /// Searches for a bank account by user id
        /// </summary>
        /// <param name="userId">User identification number</param>
        /// <returns>Found bank accounts</returns>
        IEnumerable<BankAccount> GetByUserId(string userId);

        /// <returns>All bank accounts from repository</returns>
        IEnumerable<BankAccount> GetAll();

        /// <summary>
        /// Adds a new bank account to the repository. Copies an argument
        /// </summary>
        /// <param name="account">Template bank account</param>
        void Create(BankAccount account);

        /// <summary>
        /// Searches the repository for a bank account by id
        /// and changes based on the passed
        /// </summary>
        /// <param name="account">Bank account to be changed</param>
        void Update(BankAccount account);

        /// <summary>
        /// Deletes a bank account by id
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        void Delete(string id);
    }
}
