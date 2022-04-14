namespace Minibank.Core.Domains.BankAccounts.Repositories
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
        Task<BankAccount> GetByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Searches for a bank account by user id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="userId">User identification number</param>
        /// <returns>Found bank accounts</returns>
        Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);

        ///<summary>
        ///<br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>All bank accounts</returns>
        Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new bank account. Copies an argument
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="account">Template bank account</param>
        Task CreateAsync(BankAccount account, CancellationToken cancellationToken);

        /// <summary>
        /// Searches for a bank account by id
        /// and changes based on the passed
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="account">Bank account to be changed</param>
        Task UpdateAsync(BankAccount account, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a bank account by id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        Task DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Makes an account inactive and sets a closing date
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        Task CloseAccountAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Changes account balance
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="amount">New account balance</param>
        Task UpdateBalanceAsync(string id, double amount, CancellationToken cancellationToken);

        /// <summary>
        /// Looks for the first available account from the transferred user
        /// </summary>
        /// <param name="userId">user identification number</param>
        /// <returns>True if at least one account exists</returns>
        Task<bool> ExistsByUserIdAsync(string userId, CancellationToken cancellationToken);
    }
}
