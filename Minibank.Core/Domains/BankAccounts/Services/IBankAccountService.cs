namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        /// <summary>
        /// Searches for a bank account by his id
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <returns>Found Bank account</returns>
        Task<BankAccount> GetByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Searches for a bank account by user id
        /// </summary>
        /// <param name="userId">User identification number</param>
        /// <returns>Found bank accounts</returns>
        Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);

        /// <returns>All bank accounts from repository</returns>
        Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new bank account to the repository. Copies an argument
        /// </summary>
        /// <param name="account">Template bank account</param>
        Task CreateAsync(BankAccount account, CancellationToken cancellationToken);

        /// <summary>
        /// Searches the repository for a bank account by id
        /// and changes based on the passed
        /// </summary>
        /// <param name="account">Bank account to be changed</param>
        Task UpdateAsync(BankAccount account, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a bank account by id
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        Task DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Makes an account inactive and sets a closing date
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        Task CloseAccountAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Changes account balance
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        /// <param name="amount">New account balance</param>
        Task UpdateBalanceAsync(string id, double amount, CancellationToken cancellationToken);

        /// <summary>
        /// Calculates the commission amount when transferring between two accounts
        /// </summary>
        /// <returns>Commission amount</returns>
        Task<double> CalculateCommissionAsync(double amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken);

        /// <summary>
        /// Transferring funds between accounts
        /// </summary>
        Task MoneyTransactAsync(double amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken);
    }
}
