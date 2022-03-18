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
        BankAccount GetById(string id);

        /// <summary>
        /// Searches for a bank account by user id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="userId">User identification number</param>
        /// <returns>Found bank accounts</returns>
        IEnumerable<BankAccount> GetByUserId(string userId);

        ///<summary>
        ///<br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>All bank accounts</returns>
        IEnumerable<BankAccount> GetAll();

        /// <summary>
        /// Adds a new bank account. Copies an argument
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="account">Template bank account</param>
        void Create(BankAccount account);

        /// <summary>
        /// Searches for a bank account by id
        /// and changes based on the passed
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="account">Bank account to be changed</param>
        void Update(BankAccount account);

        /// <summary>
        /// Deletes a bank account by id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        void Delete(string id);

        /// <summary>
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>List of currency codes available for registration</returns>
        IEnumerable<string>? GetValidCurrencies();

        /// <summary>
        /// Makes an account inactive and sets a closing date
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Bank account identification number</param>
        void CloseAccount(string id);

        /// <summary>
        /// Changes account balance
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="amount">New account balance</param>
        void UpdateBalance(string id, double amount);

        /// <summary>
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>The amount of commission charged in percent, like 0.02 (it's 2%)</returns>
        double GetCommissionPercent();

        /// <summary>
        /// Calculates the commission amount when transferring between two accounts
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>Commission amount</returns>
        double CalculateCommission(double amount, string fromAccountId, string toAccountId);

        /// <summary>
        /// Transferring funds between accounts
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        void MoneyTransaction(double amount, string fromAccountId, string toAccountId);
    }
}
